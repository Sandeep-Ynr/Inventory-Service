using System.Net.Http;
using Microsoft.Extensions.Logging;
using MilkMatrix.Core.Abstractions.HttpClient;
using MilkMatrix.Core.Abstractions.Logger;
using MilkMatrix.Core.Entities.Common;
using MilkMatrix.Core.Extensions;

namespace MilkMatrix.Infrastructure.Factories;

public class ClientFactory : IClientFactory
{
    private readonly IHttpClientFactory httpClientFactory;
    private ILogging logger;
    public ClientFactory(IHttpClientFactory httpClientFactory, ILogging logger)
    {
        this.httpClientFactory = httpClientFactory;
        this.logger = logger.ForContext("ServiceName", nameof(ClientFactory));
    }
    public async Task<HttpResponseMessage> GetAsync(string request, string clientName, Dictionary<string, string> headers = null!,
        CancellationToken cancellationToken = default)
    {
        try
        {
            using var httpclient = httpClientFactory.CreateClient();
            var httpRequest = new HttpRequestMessage(HttpMethod.Get, request)
                .SetHeaders(HttpRequestExtensions.AddHeaders(clientName, headers));

            var httpResponse = await httpclient.SendAsync(httpRequest, cancellationToken).ConfigureAwait(false);
            if (!httpResponse.IsSuccessStatusCode)
                logger.LogError(string.Format(ConstantStrings.GetError, clientName));

            return httpResponse;
        }
        catch (UnauthorizedAccessException ex)
        {
            logger.LogError(ex.Message, ex);
            throw new Exception(StatusCodeMessage.Unauthorized);
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message, ex);
            throw new Exception(StatusCodeMessage.InternalServerError);
        }
    }

    public async Task<T> PostAsync<T>(string request,
        string clientName,
        HttpContent httpContent,
        Dictionary<string, string> headers = null!,
        CancellationToken cancellationToken = default)
    {
        try
        {
            using var httpclient = httpClientFactory.CreateClient(clientName);
            var httpRequest = new HttpRequestMessage(HttpMethod.Post, request) { Content = httpContent }
                .SetHeaders(HttpRequestExtensions.AddHeaders(clientName, headers));

            var httpResponse = await httpclient.SendAsync(httpRequest, cancellationToken).ConfigureAwait(false);
            var result = await httpResponse.Content.ReadAsAsync<T>();

            if (result == null)
            {
                logger.LogError(string.Format(ConstantStrings.PostError, clientName));
            }
            return result;
        }
        catch (UnauthorizedAccessException ex)
        {
            logger.LogError(ex.Message, ex);
            throw new Exception(StatusCodeMessage.Unauthorized);
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message, ex);
            throw new Exception(StatusCodeMessage.InternalServerError);
        }
    }
}
