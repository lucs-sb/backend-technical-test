using Application.DTOs;

namespace Application.Interfaces;

public interface IGitHubIntegration
{
    Task<List<RepositorioDTO>> ListarRepositoriosDoUsuario(string usuario);
    Task<List<RepositorioDTO>> BuscarRepositoriosPeloNome(string nome);
}
