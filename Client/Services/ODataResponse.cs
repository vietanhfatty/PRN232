using System.Text.Json.Serialization;

namespace Client.Services;

public class ODataResponse<T>
{
    [JsonPropertyName("@odata.context")]
    public string? ODataContext { get; set; }

    [JsonPropertyName("value")]
    public T? Value { get; set; }
}
