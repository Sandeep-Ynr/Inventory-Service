using System.Net;
using MilkMatrix.Core.Abstractions.Logger;
using MilkMatrix.Core.Entities.Common;
using MilkMatrix.Notifications.Contracts;
using MilkMatrix.Notifications.Models.Enums;
using MilkMatrix.Notifications.Models.OTP.Request;
using MilkMatrix.Notifications.Models.OTP.Response;

namespace MilkMatrix.Notifications.Implementation;

public class OtpService : IOtpService
{
    private ILogging logger;
    private readonly ISMSService smsService;
    private readonly IEmailService emailService;

    public OtpService(ILogging logger,
        ISMSService smsService,
        IEmailService emailService
        )
    {
        this.smsService = smsService;
        this.emailService = emailService;
        this.logger = logger.ForContext("ServiceName", nameof(OtpService));
    }
    public async Task<TResponse> SendAsync<TRequest, TResponse>(TRequest request)
    {
        try
        {
            if (request is OTPRequest otpRequest)
            {
                var response = await SendAsync(otpRequest);
                return (TResponse)(object)response;
            }
            else
            {
                throw new InvalidCastException("Invalid request type. Expected OTPRequest.");
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message, ex);
            return (TResponse)(object)new OTPResponse
            {
                Code = (int)HttpStatusCode.InternalServerError,
                Message = StatusCodeMessage.OtpErrorMessage
            };
        }
    }

    private async Task<OTPResponse> SendAsync(OTPRequest request)
    {
        switch (request.OTPType)
        {
            case OTPType.Both:
                try
                {
                    var smsTask = await smsService.SendSMSAsync(new SMSRequest { MobileNumber = request.MobileNumber });
                    var emailTask = await emailService.SendEmailAsync(new EmailRequest { EmailId = request.EmailId, TemplateType = request.TemplateType });
                    //var otpResponse = await Task.WhenAll(smsTask, emailTask).ConfigureAwait(false);
                    return emailTask;
                }
                catch (Exception) { return new OTPResponse { Code = (int)HttpStatusCode.InternalServerError, Message = StatusCodeMessage.InternalServerError }; }
            case OTPType.Email:
                return await emailService.SendEmailAsync(new EmailRequest { EmailId = request.EmailId, TemplateType = request.TemplateType });
            case OTPType.Sms:
                return await smsService.SendSMSAsync(new SMSRequest { MobileNumber = request.MobileNumber });
            case OTPType.SystemEmail:
                try
                {
                    var response = await emailService.SendSystemsEmailAsync(new SystemEmailRequest { EmailId = request.EmailId, TemplateType = request.TemplateType, Content = request.Content });
                    return new OTPResponse { Code = response ? (int)HttpStatusCode.OK : (int)HttpStatusCode.InternalServerError, Message = response ? StatusCodeMessage.MailSuccessMessage : StatusCodeMessage.InternalServerError };
                }
                catch (Exception) { return new OTPResponse { Code = (int)HttpStatusCode.InternalServerError, Message = StatusCodeMessage.InternalServerError }; }
            default:
                return new OTPResponse { Code = (int)HttpStatusCode.InternalServerError, Message = StatusCodeMessage.OtpErrorMessage };
        }
    }
}
