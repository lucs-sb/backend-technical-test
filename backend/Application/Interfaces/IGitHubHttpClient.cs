using Application.DTOs;

namespace Application.Interfaces;

public interface IGitHubHttpClient
{
    Task<List<RepositorioDTO>> ListarRepositoriosDoUsuario(
        string usuario);

    Task<PaginacaoResultadoDTO<RepositorioDTO>> BuscarRepositoriosPeloNome(
        string nome, int pagina, int tamanhoPagina);
}
