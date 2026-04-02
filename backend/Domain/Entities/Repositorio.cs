namespace Domain.Entities;

public sealed class Repositorio
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string Nome { get; set; }
    public required string HtmlUrl { get; set; }
    public int QuantidadeEstrelas { get; set; }
    public int QuantidadeForks { get; set; }
    public int QuantidadeObservadores { get; set; }
}
