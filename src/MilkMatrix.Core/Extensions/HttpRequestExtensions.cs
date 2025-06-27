using MilkMatrix.Core.Entities.Common;

namespace MilkMatrix.Core.Extensions;

public static class HttpRequestExtensions
{
  public static Dictionary<string, string> AddHeaders(string clientName, Dictionary<string, string> headers = null!)
  {
    if (headers != null && !headers.ContainsKey(ConstantStrings.UserAgent))
    {
      headers.Add(ConstantStrings.UserAgent, $"{clientName}-Client");
      return headers;
    }
    else
    {
      return new Dictionary<string, string> {
                {ConstantStrings.UserAgent, $"{clientName}-Client" }
            };
    }
  }

  public static HttpRequestMessage SetHeaders(this HttpRequestMessage requestMessage, Dictionary<string, string> headers)
  {
    foreach ((string key, string value) in headers)
    {
      if (requestMessage.Headers.Contains(key))
        requestMessage.Headers.Remove(key);
      if (!string.IsNullOrEmpty(value))
        requestMessage.Headers.Add(key, value);
    }

    return requestMessage;
  }

  public static HttpRequestMessage SetAuthorizationHeader(this HttpRequestMessage requestMessage, string token)
  {
    if (requestMessage.Headers.Contains(ConstantStrings.Authorization))
      requestMessage.Headers.Remove(ConstantStrings.Authorization);

    if (!string.IsNullOrEmpty(token))
      requestMessage.Headers.Add(ConstantStrings.Authorization, token);

    return requestMessage;
  }
}
