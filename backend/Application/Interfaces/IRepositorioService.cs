using Application.DTOs;

namespace Application.Interfaces;

public interface IRepositorioService
{
    Task<List<RepositorioDTO>> ListarRepositoriosDoUsuario(string usuario);
    Task<PaginacaoResultadoDTO<RepositorioDTO>> BuscarRepositoriosPeloNome(string nome, int pagina, int tamanhoPagina);
    void AdicionarFavorito(RepositorioDTO repositorioDTO);
    List<FavoritoDTO> ListarFavoritos();
}