namespace MilkMatrix.Core.Abstractions.HttpClient;

public interface IClientFactory
{
    Task<HttpResponseMessage> GetAsync(string request, string clientName, Dictionary<string, string> headers = null!, CancellationToken cancellationToken = default);
    Task<T> PostAsync<T>(string request, string clientName, HttpContent httpContent, Dictionary<string, string> headers = null!,
        CancellationToken cancellationToken = default);
}
