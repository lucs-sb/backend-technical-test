using Application.DTOs;

namespace Application.Interfaces;

public interface IGitHubHttpClient
{
    Task<List<RepositorioDTO>> ListarRepositoriosDoUsuario(string usuario);
    Task<List<RepositorioDTO>> BuscarRepositoriosPeloNome(string nome);
}
