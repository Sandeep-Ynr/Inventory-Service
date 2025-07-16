using System.Net;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MilkMatrix.Core.Abstractions.Logger;
using MilkMatrix.Core.Abstractions.Repository.Factories;
using MilkMatrix.Core.Entities.Common;
using MilkMatrix.Core.Entities.Config;
using MilkMatrix.Core.Entities.Enums;
using MilkMatrix.Notifications.Common.Constants;
using MilkMatrix.Notifications.Common.Extensions;
using MilkMatrix.Notifications.Contracts;
using MilkMatrix.Notifications.Models;
using MilkMatrix.Notifications.Models.Config;
using MilkMatrix.Notifications.Models.Enums;
using MilkMatrix.Notifications.Models.OTP.Request;
using MilkMatrix.Notifications.Models.OTP.Response;
using static MilkMatrix.Notifications.Models.Queries.ConfigurationSettings;

namespace MilkMatrix.Notifications.Implementation;

public class EmailService : IEmailService
{
    private ILogging logger;
    private IRepositoryFactory repositoryFactory;
    private AppConfig appConfig;
    private EmailConfig emailConfig;

    private readonly IHostEnvironment hostingEnvironment;
    public EmailService(ILogging logger, IRepositoryFactory repositoryFactory, IOptions<AppConfig> appConfig,
        IHostEnvironment hostingEnvironment,
        IOptions<EmailConfig> emailConfig
        )
    {
        this.repositoryFactory = repositoryFactory;
        this.appConfig = appConfig.Value ?? throw new ArgumentNullException(nameof(AppConfig));
        this.hostingEnvironment = hostingEnvironment;
        this.logger = logger.ForContext("ServiceName", nameof(EmailService));
        this.emailConfig = emailConfig.Value ?? throw new ArgumentNullException(nameof(EmailConfig));
    }

    public async Task<OTPResponse> SendEmailAsync(EmailRequest emailRequest)
    {
        var verificationCode = appConfig.AllowToSendMail
            ? FixedStrings.RandomNumberLength.GenerateRendomNumber()
        : FixedStrings.DefaultVerificationCode;

        verificationCode = emailRequest.VerificationCode ?? verificationCode;

        var repoFact = repositoryFactory.ConnectDapper<string>(DbConstants.Main);
        var isUserExist = (await repoFact.QueryAsync<int>(UserSpName.GetUserId, new Dictionary<string, object> { { "Id", emailRequest.EmailId } }, null))?.FirstOrDefault() > 0;
        if (!isUserExist)
        {
            return new OTPResponse { Message = StatusCodeMessage.IdNotFound, Code = (int)HttpStatusCode.NotFound };
        }
        else
        {
            var otpResponse = new OTPResponse();
            if (emailConfig.OtpNeed == OTPEnum.Skip)
            {
                otpResponse = new OTPResponse { Code = (int)HttpStatusCode.OK, Message = StatusCodeMessage.Ok, IsOtpSent = true, IsOtpSkiped = true, OtpStatus = true, Otp = verificationCode };
            }
            else
            {
                var emailSettings = await GetEmailConfiguration((int)emailRequest.TemplateType);

                if (emailSettings == null)
                {
                    logger.LogError("Email Settings error");
                    return new OTPResponse { Message = StatusCodeMessage.InternalServerError, Code = (int)HttpStatusCode.InternalServerError };
                }
                emailSettings.Content4 = verificationCode.ToString();
                emailSettings.MailTo = emailRequest.EmailId;

                var responseFromEmail = emailSettings.SendMail(appConfig.AllowToSendMail, logger, hostingEnvironment.ContentRootPath);

                otpResponse = responseFromEmail.AutoGenenateOtpResponseModel(verificationCode);
            }

            var result = otpResponse.IsOtpSent ?
                    (OTPResponseEnum)await repoFact.ExecuteScalarAsync(NotificationSettings.SendOtpToUser, CrudActionType.Create.PrepareSendEmailOtpParameters(
                    emailRequest.EmailId, verificationCode.ToString(), FixedStrings.BlankValue, otpResponse.OtpStatus)) : OTPResponseEnum.Error;
            verificationCode = result == OTPResponseEnum.Success && emailConfig.OtpNeed == OTPEnum.Skip ? verificationCode : 0;
            return otpResponse.PrepareResponse(result, verificationCode);
        }
    }

    public async Task<bool> SendSystemsEmailAsync(SystemEmailRequest emailRequest)
    {
        var verificationCode = appConfig.AllowToSendMail
            ? FixedStrings.RandomNumberLength.GenerateRendomNumber()
            : FixedStrings.DefaultVerificationCode;

        var emailSettings = await GetEmailConfiguration((int)emailRequest.TemplateType);

        if (emailSettings == null)
        {
            logger.LogError("Email Settings error");
            return false;
        }
        emailSettings.MailTo = !string.IsNullOrEmpty(emailRequest.EmailId) ? emailRequest.EmailId : emailSettings.MailTo;
        emailSettings.Content2 = emailRequest.Content;

        var responseFromEmail = emailSettings.SendMail(appConfig.AllowToSendMail, logger, hostingEnvironment.ContentRootPath);

        return responseFromEmail.Code == (int)HttpStatusCode.OK;
    }

    private async Task<EmailSettings?> GetEmailConfiguration(int templateType) => (await repositoryFactory.ConnectDapper<EmailSettings>(DbConstants.Main).QueryAsync<EmailSettings>(NotificationSettings.GetMailDetails,
                ((int)CrudOperationType.Read).PrepareEmailSettingsParameters(templateType.ToString())
                , null))?.FirstOrDefault();
}
