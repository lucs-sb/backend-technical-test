namespace Application.DTOs;

public sealed record PaginacaoResultadoDTO<T>(
    int TotalItens,
    int Pagina,
    int TamanhoPagina,
    IReadOnlyList<T> Itens
);
