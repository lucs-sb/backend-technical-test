using Application.DTOs;
using Domain.Entities;

namespace Application.Interfaces;

public interface IGitHubHttpClient
{
    Task<List<RepositorioDTO>> ListarRepositoriosDoUsuario(
        string usuario);

    Task<PaginacaoResultadoDTO<Repositorio>> BuscarRepositoriosPeloNome(
        string nome, int pagina, int tamanhoPagina);
}
