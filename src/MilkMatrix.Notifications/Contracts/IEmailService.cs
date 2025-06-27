using MilkMatrix.Notifications.Models.OTP.Request;
using MilkMatrix.Notifications.Models.OTP.Response;

namespace MilkMatrix.Notifications.Contracts;

public interface IEmailService
{
  Task<OTPResponse> SendEmailAsync(EmailRequest emailRequest);

  Task<bool> SendSystemsEmailAsync(SystemEmailRequest emailRequest);
}
