namespace Api.Models;

public sealed class RepositorioModel
{
    public string Nome { get; set; } = default!;
    public string HtmlUrl { get; set; } = default!;
    public int QuantidadeEstrelas { get; set; }
    public int QuantidadeForks { get; set; }
    public int QuantidadeObservadores { get; set; }
}