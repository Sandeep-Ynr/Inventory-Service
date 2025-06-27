using MilkMatrix.Notifications.Models.OTP.Request;
using MilkMatrix.Notifications.Models.OTP.Response;

namespace MilkMatrix.Notifications.Contracts;

public interface ISMSService
{
    Task<OTPResponse> SendSMSAsync(SMSRequest request);
}
