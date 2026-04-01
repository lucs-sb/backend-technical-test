using Application.DTOs;
using Application.Interfaces;

namespace Application.Services;

public sealed class RepositorioService : IRepositorioService
{
    private readonly HttpClient _http;

    public RepositorioService()
    {
        _http = new HttpClient();
        _http.DefaultRequestHeaders.UserAgent.ParseAdd("request");
    }

    public async Task<List<object>> ListarRepositoriosDoUsuario(string usuario)
    {
        // TODO
        // Seu código aqui
        throw new NotImplementedException("Implementar lógica para listar repositórios.");
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