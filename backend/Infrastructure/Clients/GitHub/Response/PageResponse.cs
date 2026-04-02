using System.Text.Json.Serialization;

namespace Infrastructure.Clients.GitHub.Response;

internal sealed record PageResponse<T>(
    [property: JsonPropertyName("total_count")] int QuantidadeTotal,
    [property: JsonPropertyName("items")] List<T> Items);