using System.Text.Json.Serialization;

namespace Infrastructure.Clients.GitHub.Response;

internal sealed record RepositoryResponse(
    [property: JsonPropertyName("name")] string Nome,
    [property: JsonPropertyName("html_url")] string HtmlUrl,
    [property: JsonPropertyName("stargazers_count")] int QuantidadeEstrelas,
    [property: JsonPropertyName("forks_count")] int QuantidadeForks,
    [property: JsonPropertyName("watchers_count")] int QuantidadeObservadores
);