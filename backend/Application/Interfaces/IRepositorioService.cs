using Application.DTOs;

namespace Application.Interfaces;

public interface IRepositorioService
{
    Task<List<object>> ListarRepositoriosDoUsuario(string usuario);
    Task AdicionarFavorito(FavoritoDTO favoritoDTO);
    Task<List<FavoritoDTO>> ListarFavoritos();
}