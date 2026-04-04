namespace Application.DTOs;

public sealed class PaginacaoResultadoDTO<T>
{
    public int TotalItens { get; set; }
    public int Pagina { get; set; }
    public int TamanhoPagina { get; set; }
    public List<T> Itens { get; set; } = new List<T>();
}