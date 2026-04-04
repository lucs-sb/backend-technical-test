namespace Api.Models;

public sealed class BuscarRepositoriosQueryModel : PaginacaoQueryModel
{
    public string Nome { get; set; } = default!;
}
