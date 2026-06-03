using System.Net.Http.Json;
using System.Text.Json;

namespace Client.Services;

public class ApiService
{
    private readonly HttpClient _httpClient;

    public ApiService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("ApiClient");
    }

    public async Task<T?> GetAsync<T>(string path)
    {
        var response = await _httpClient.GetAsync(path);
        if (!response.IsSuccessStatusCode)
        {
            return default;
        }
        return await response.Content.ReadFromJsonAsync<T>();
    }

    public async Task<HttpResponseMessage> PostAsync<T>(string path, T data)
    {
        return await _httpClient.PostAsJsonAsync(path, data);
    }

    public async Task<HttpResponseMessage> PutAsync<T>(string path, T data)
    {
        return await _httpClient.PutAsJsonAsync(path, data);
    }

    public async Task<HttpResponseMessage> DeleteAsync(string path)
    {
        return await _httpClient.DeleteAsync(path);
    }
}
