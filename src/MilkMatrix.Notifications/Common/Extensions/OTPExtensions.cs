using System.Net;
using MilkMatrix.Core.Entities.Common;
using MilkMatrix.Core.Entities.Enums;
using MilkMatrix.Notifications.Common.Constants;
using MilkMatrix.Notifications.Models.Enums;
using MilkMatrix.Notifications.Models.OTP.Response;
using static MilkMatrix.Core.Entities.Common.Constants;

namespace MilkMatrix.Notifications.Common.Extensions;

public static class OTPExtensions
{
    public static int GenerateRendomNumber(this int length)
    {
        int rendomNumber = 0;
        const string chars = "0123456789";
        var random = new Random();
        var output = new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        rendomNumber = Convert.ToInt32(output.TrimStart('0'));
        return rendomNumber;
    }
    public static Dictionary<string, object> PrepareSMSConfigurationParameters(this OTPActionType actionType, int id, bool isActive) =>
        new Dictionary<string, object>
        {
            { "ActionType", (int)actionType }, { "Id",id }, {"IsStatus",isActive }
        };

    public static Dictionary<string, object> PrepareSendOtpParameters(this CrudActionType actionType, string mobile, string otp, string deviceToken, bool otpStatus) =>
       new Dictionary<string, object>
       {
        { "ActionType", (int)actionType }, { "Id",mobile },{ "DeviceToken",deviceToken },{ "otp",otp }, {"OtpStatus",otpStatus }
       };

    public static Dictionary<string, object> PrepareSendEmailOtpParameters(this CrudActionType actionType, string email, string otp, string deviceToken, bool otpStatus) =>
       new Dictionary<string, object>
       {
        { "ActionType", (int)actionType }, { "Id",email },{ "DeviceToken",deviceToken },{ "otp",otp }, {"OtpStatus",otpStatus }
       };

    public static string FormatUrl(this string url, string mobile, string? senderId, string smsBody) => url.Replace(FixedStrings.Mobile, mobile)
                                 .Replace(FixedStrings.SmsBody, WebUtility.UrlEncode(smsBody))
                                 .Replace(FixedStrings.SenderId, senderId)
                                 .Replace(FixedStrings.DefaultDateTimeFormat, DateTime.Now.ToString(FixedStrings.DefaultDateTimeFormat));

    public static Dictionary<string, object> PrepareEmailSettingsParameters(this int actionType, string uniqueId) => new Dictionary<string, object>
    {
        { "ActionType", actionType },
        { "IdList",uniqueId }
    };

    public static string CreateSMSContent(this string content, SMSContent model)
    {
        string str = content;
        if (model.FullName != null && model.FullName != "")
        {
            str = str.Replace("#NAME#", model.FullName);
        }
        else
            str = str.Replace("#NAME#", "");
        if (model.Content1 != null && model.Content1 != "")
        {
            str = str.Replace("{#var#}", model.Content1);
        }
        else
            str = str.Replace("{#var#}", "");

        if (model.Content2 != null && model.Content2 != "")
        {
            str = str.Replace("#CONTENT2#", model.Content2);
        }
        else
            str = str.Replace("#CONTENT2#", "");

        if (model.Content3 != null && model.Content3 != "")
        {
            str = str.Replace("#CONTENT3#", model.Content3);
        }
        else
            str = str.Replace("#CONTENT3#", "");

        if (model.Content4 != null && model.Content4 != "")
        {
            str = str.Replace("#CONTENT4#", model.Content4);
        }
        else
            str = str.Replace("#CONTENT4#", "");

        if (model.Content5 != null && model.Content5 != "")
        {
            str = str.Replace("#CONTENT5#", model.Content5);
        }
        else
            str = str.Replace("#CONTENT5#", "");

        if (model.Content6 != null && model.Content6 != "")
        {
            str = str.Replace("#CONTENT6#", model.Content6);
        }
        else
            str = str.Replace("#CONTENT6#", "");
        return str;
    }

    public static OTPResponse AutoGenenateOtpResponseModel(this StatusCode httpStatusCode, int Otp)
    {
        switch ((HttpStatusCode)httpStatusCode.Code)
        {
            case HttpStatusCode.OK:
                return new OTPResponse() { Code = (int)HttpStatusCode.OK, Message = StatusCodeMessage.Ok, IsOtpSent = true, OtpStatus = true, Otp = Otp };
            case HttpStatusCode.Created:
                return new OTPResponse() { Code = (int)HttpStatusCode.Created, Message = StatusCodeMessage.Created, IsOtpSent = true, OtpStatus = true, Otp = Otp };
            case HttpStatusCode.Accepted:
                return new OTPResponse() { Code = (int)HttpStatusCode.Accepted, Message = StatusCodeMessage.Accepted, IsOtpSent = true, OtpStatus = true, Otp = Otp };
            case HttpStatusCode.BadRequest:
                return new OTPResponse() { Code = (int)HttpStatusCode.BadRequest, Message = StatusCodeMessage.BadRequest, IsOtpSent = false, OtpStatus = false };
            case HttpStatusCode.Unauthorized:
                return new OTPResponse() { Code = (int)HttpStatusCode.Unauthorized, Message = StatusCodeMessage.Unauthorized, IsOtpSent = false, OtpStatus = false };
            case HttpStatusCode.NotFound:
                return new OTPResponse() { Code = (int)HttpStatusCode.NotFound, Message = StatusCodeMessage.NotFound, IsOtpSent = false, OtpStatus = false };
            case HttpStatusCode.Conflict:
                return new OTPResponse() { Code = (int)HttpStatusCode.Conflict, Message = StatusCodeMessage.Conflict, IsOtpSent = false, OtpStatus = false };
            case HttpStatusCode.InternalServerError:
                return new OTPResponse() { Code = (int)HttpStatusCode.InternalServerError, Message = httpStatusCode.Message, IsOtpSent = false, OtpStatus = false };
            case HttpStatusCode.ServiceUnavailable:
                return new OTPResponse() { Code = (int)HttpStatusCode.ServiceUnavailable, Message = StatusCodeMessage.ServiceUnavailable, IsOtpSent = false, OtpStatus = false };
            default:
                return new OTPResponse() { Code = (int)HttpStatusCode.ServiceUnavailable, Message = StatusCodeMessage.ServiceUnavailable, IsOtpSent = false, OtpStatus = false };
        }
    }

    public static OTPResponse PrepareResponse(this OTPResponse response, OTPResponseEnum result, int otp)
    {
        switch (result)
        {
            case OTPResponseEnum.Success:
                response.Otp = response.IsOtpSkiped ? otp : 0;
                response.Message = SuccessMessage.OTPSuccess;
                return response;
            case OTPResponseEnum.TokenNotMatch:
                response.Message = ErrorMessage.DeviceTokenNotMatched;
                return response;
            case OTPResponseEnum.MobileNotExists:
                response.Message = ErrorMessage.MobileNumberNotExists;
                return response;
            case OTPResponseEnum.Error:
                response.Message = ErrorMessage.Failed;
                return response;
            default:
                response.Message = StatusCodeMessage.InternalServerError;
                return response;
        }
    }
}
