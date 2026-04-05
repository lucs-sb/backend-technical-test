using Application.DTOs;

namespace Tests.TestBuilders;

public sealed class PaginacaoResultadoBuilder<T>
{
    private int _totalItens = 0;
    private int _pagina = 1;
    private int _tamanhoPagina = 10;
    private List<T> _itens = new();

    public PaginacaoResultadoBuilder<T> ComTotalItens(int totalItens)
    {
        _totalItens = totalItens;
        return this;
    }

    public PaginacaoResultadoBuilder<T> ComPagina(int pagina)
    {
        _pagina = pagina;
        return this;
    }

    public PaginacaoResultadoBuilder<T> ComTamanhoPagina(int tamanhoPagina)
    {
        _tamanhoPagina = tamanhoPagina;
        return this;
    }

    public PaginacaoResultadoBuilder<T> ComItens(IEnumerable<T> itens)
    {
        _itens = itens.ToList();
        return this;
    }

    public PaginacaoResultadoDTO<T> Build()
    {
        return new PaginacaoResultadoDTO<T>
        {
            TotalItens = _totalItens,
            Pagina = _pagina,
            TamanhoPagina = _tamanhoPagina,
            Itens = _itens
        };
    }
}
