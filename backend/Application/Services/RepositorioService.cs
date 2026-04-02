using Application.DTOs;
using Application.Interfaces;

namespace Application.Services;

public sealed class RepositorioService : IRepositorioService
{
    private readonly IGitHubHttpClient _gitHubHttpClient;

    public RepositorioService(IGitHubHttpClient gitHubHttpClient)
    {
        _gitHubHttpClient = gitHubHttpClient;
    }

    public async Task<List<RepositorioDTO>> ListarRepositoriosDoUsuario(string usuario)
    {
        return await _gitHubHttpClient.ListarRepositoriosDoUsuario(usuario);
    }

    public async Task<List<RepositorioDTO>> BuscarRepositoriosPeloNome(string nome)
    {
        return await _gitHubHttpClient.BuscarRepositoriosPeloNome(nome);
    }

    public async Task AdicionarFavorito(FavoritoDTO favoritoDTO)
    {
        // TODO
        // Seu código aqui
    }

    public async Task<List<FavoritoDTO>> ListarFavoritos()
    {
        // TODO
        // Seu código aqui
        throw new NotImplementedException("Implementar lógica para listar favoritos.");
    }
}