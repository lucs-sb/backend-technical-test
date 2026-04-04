namespace Api.Models;

public abstract class PaginacaoQueryModel
{
    private int? _pagina;
    private int? _tamanhoPagina;
    public int? Pagina
    {
        get => _pagina ?? 1;
        set => _pagina = value;
    }
    public int? TamanhoPagina
    {
        get => _tamanhoPagina ?? 10;
        set => _tamanhoPagina = value;
    }
}
