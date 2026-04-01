namespace Application.Options;

public sealed class IntegrationOptions
{
    public const string SectionName = "IntegrationOptions";
    
    public string GitHubUri { get; set; } = default!;
}
