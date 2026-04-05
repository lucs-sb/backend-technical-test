using Application.DTOs;
using Domain.Entities;

namespace Application.Interfaces;

public interface IGitHubHttpClient
{
    Task<PaginacaoResultadoDTO<Repositorio>> BuscarRepositoriosPeloNome(
        string nome, int pagina, int tamanhoPagina);
}
