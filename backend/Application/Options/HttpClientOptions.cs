namespace Application.Options;

public sealed class HttpClientOptions
{
    public const string SectionName = "HttpClientOptions";
    
    public string GitHubUri { get; set; } = default!;
}
