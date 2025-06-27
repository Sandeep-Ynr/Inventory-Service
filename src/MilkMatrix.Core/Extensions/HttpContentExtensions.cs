using System.Text.Json;

namespace MilkMatrix.Core.Extensions;

public static class HttpContentExtensions
{
    public static async Task<T> ReadAsAsync<T>(this HttpContent content) =>
        await JsonSerializer.DeserializeAsync<T>(await content.ReadAsStreamAsync());
}
