using System.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MilkMatrix.Core.Abstractions.HttpClient;
using MilkMatrix.Core.Abstractions.Logger;
using MilkMatrix.Core.Abstractions.Repository.Factories;
using MilkMatrix.Core.Entities.Common;
using MilkMatrix.Core.Entities.Config;
using MilkMatrix.Notifications.Common.Constants;
using MilkMatrix.Notifications.Common.Extensions;
using MilkMatrix.Notifications.Contracts;
using MilkMatrix.Notifications.Models.Config;
using MilkMatrix.Notifications.Models.Enums;
using MilkMatrix.Notifications.Models.OTP.Request;
using MilkMatrix.Notifications.Models.OTP.Response;
using static MilkMatrix.Notifications.Models.Queries.ConfigurationSettings;

namespace MilkMatrix.Notifications.Implementation;

public class SMSService : ISMSService
{
    private SMSConfig smsConfig;
    private AppConfig appConfig;
    private IRepositoryFactory dbContext;
    private IClientFactory httpClient;
    private ILogging logger;
    private IConfiguration configuration;

    public SMSService(IOptions<SMSConfig> smsConfig,
        IOptions<AppConfig> appConfig,
        IRepositoryFactory dbContext,
        IClientFactory httpClient,
        ILogging logger,
        IConfiguration configuration
        )
    {
        this.smsConfig = smsConfig.Value ?? throw new ArgumentNullException(nameof(SMSConfig));
        this.dbContext = dbContext;
        this.appConfig = appConfig.Value ?? throw new ArgumentNullException(nameof(AppConfig));
        this.httpClient = httpClient;
        this.configuration = configuration;
        this.logger = logger.ForContext("ServiceName", nameof(SMSService));
    }
    public async Task<OTPResponse> SendSMSAsync(SMSRequest request)
    {
        try
        {
            var otp = appConfig.AllowToCreateOTP ? FixedStrings.OtpLength.GenerateRendomNumber() : 123456;

            var isUserExist = (await dbContext.ConnectDapper<string>(DbConstants.Main).QueryAsync<int>(UserSpName.GetUserId, new Dictionary<string, object> { { "Mobile", request.MobileNumber } }, null))?.FirstOrDefault() > 0;

            if (!isUserExist)
            {
                return new OTPResponse().PrepareResponse(OTPResponseEnum.MobileNotExists, 0);
            }
            else
            {
                var smsConfiguration = await GetSmsConfiguration();

                var response = smsConfiguration.Item1 != null && smsConfiguration.Item2 != null && smsConfiguration.Item3 != null && appConfig.AllowToCreateOTP
                    ? await PrepareToSendOtpAsync(smsConfig.OtpNeed, request.MobileNumber, otp, smsConfiguration.Item2, smsConfiguration.Item1, smsConfiguration.Item3)
                    : new OTPResponse { Code = (int)HttpStatusCode.InternalServerError, Message = StatusCodeMessage.InternalServerError };

                var result = response.IsOtpSent ?
                    (OTPResponseEnum)await dbContext.ConnectDapper<string>(DbConstants.Main).ExecuteScalarAsync(NotificationSettings.SendOtpToUser, CrudOperationType.Insert.PrepareSendOtpParameters(
                    request.MobileNumber, otp.ToString(), FixedStrings.BlankValue, response.OtpStatus)) : OTPResponseEnum.Error;

                return response.PrepareResponse(result, otp);
            }

        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message, ex);
            return new OTPResponse { Code = (int)HttpStatusCode.InternalServerError, Message = StatusCodeMessage.InternalServerError };
        }
    }



    private async Task<(SMSConfiguration, SMSConfiguration, SMSMerchantDetails)> GetSmsConfiguration()
    {
        var successContentFromDb = (await dbContext.ConnectDapper<SMSConfiguration>(DbConstants.Main).QueryAsync<SMSConfiguration>(NotificationSettings.GetSmsConfiguration,
                smsConfig.ActionType.PrepareSMSConfigurationParameters((int)OTPEnum.Success, true)
                , null))?.FirstOrDefault();

        var failureContentFromDb = (await dbContext.ConnectDapper<SMSConfiguration>(DbConstants.Main).QueryAsync<SMSConfiguration>(NotificationSettings.GetSmsConfiguration,
    smsConfig.ActionType.PrepareSMSConfigurationParameters((int)OTPEnum.Failure, true)
    , null))?.FirstOrDefault();

        var merchantDetails = (await dbContext.ConnectDapper<string>(DbConstants.Main).QueryAsync<SMSMerchantDetails>(NotificationSettings.GetSmsMerchantDetails, null, null))?.FirstOrDefault();

        return (successContentFromDb!, failureContentFromDb!, merchantDetails!);
    }

    private async Task<OTPResponse> PrepareToSendOtpAsync(OTPEnum result,
        string mobile,
        int otp,
        SMSConfiguration successContentFromDb,
        SMSConfiguration failureContentFromDb,
        SMSMerchantDetails merchantDetails)
    {
        var otpFinalResponse = new OTPResponse();
        var successContentModel = new SMSContent { Content1 = otp.ToString() };
        var successMessage = successContentFromDb!.SmsBody!.CreateSMSContent(successContentModel);
        var failMessage = failureContentFromDb!.SmsBody!.CreateSMSContent(successContentModel);
        switch (result)
        {
            case OTPEnum.Success:
                await SendSmsAsync(mobile, otpFinalResponse, successContentModel, merchantDetails, successMessage, failMessage);
                return otpFinalResponse;
            case OTPEnum.Skip:
            case OTPEnum.Testing:
                otpFinalResponse.IsOtpSkiped = true;
                otpFinalResponse.OtpStatus = true;
                otpFinalResponse.IsOtpSent = true;
                otpFinalResponse.Code = (int)HttpStatusCode.OK;
                otpFinalResponse.Message = successMessage;
                return otpFinalResponse;
            default:
                otpFinalResponse.Code = (int)HttpStatusCode.InternalServerError;
                otpFinalResponse.Message = failMessage;
                return otpFinalResponse;
        }
    }

    private async Task SendSmsAsync(string mobile,
        OTPResponse otpFinalResponse,
        SMSContent successContentModel,
        SMSMerchantDetails? merchantDetails,
        string successMessage,
        string failMessage)
    {
        try
        {
            var url = merchantDetails?.Url;
            var smsBody = successMessage.CreateSMSContent(successContentModel);
            url = url?.FormatUrl(mobile, merchantDetails?.SenderId, smsBody);

            var response = await httpClient.GetAsync(url!, appConfig.ClientName);
            if (response != null && response.IsSuccessStatusCode)
            {
                otpFinalResponse.IsOtpSent = true;
                otpFinalResponse.OtpStatus = true;
                otpFinalResponse.Code = (int)HttpStatusCode.OK;
                otpFinalResponse.Message = successMessage;
            }
            else
            {
                otpFinalResponse.Code = (int)HttpStatusCode.InternalServerError;
                otpFinalResponse.Message = StatusCodeMessage.InternalServerError;
            }
        }
        catch (Exception ex)
        {
            otpFinalResponse.Code = (int)HttpStatusCode.InternalServerError;
            otpFinalResponse.Message = failMessage;
            logger.LogError(ex.Message, ex);
        }
    }
}
