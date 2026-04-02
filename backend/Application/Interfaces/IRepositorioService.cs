using Application.DTOs;

namespace Application.Interfaces;

public interface IRepositorioService
{
    Task<List<RepositorioDTO>> ListarRepositoriosDoUsuario(string usuario);
    Task<List<RepositorioDTO>> BuscarRepositoriosPeloNome(string nome);
    void AdicionarFavorito(RepositorioDTO repositorioDTO);
    List<FavoritoDTO> ListarFavoritos();
}