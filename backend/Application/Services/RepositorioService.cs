using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Domain.Rules;
using Mapster;

namespace Application.Services;

public sealed class RepositorioService : IRepositorioService
{
    private readonly IGitHubHttpClient _gitHubHttpClient;
    private readonly IRepositorioStore _repositorioStore;

    public RepositorioService(IGitHubHttpClient gitHubHttpClient, IRepositorioStore repositorioStore)
    {
        _gitHubHttpClient = gitHubHttpClient;
        _repositorioStore = repositorioStore;
    }

    public async Task<PaginacaoResultadoDTO<RepositorioDTO>> BuscarRepositoriosPeloNome(
        string nome, int pagina, int tamanhoPagina)
    {
        PaginacaoResultadoDTO<Repositorio> paginacaoResultadoDTO = await _gitHubHttpClient.BuscarRepositoriosPeloNome(nome, pagina, tamanhoPagina);

        List<RepositorioDTO> repositorioDTOs = paginacaoResultadoDTO.Itens
            .OrderByDescending(RepositorioRelevanciaCalculator.CalcularRelevancia)
            .Select(r => r.Adapt<RepositorioDTO>())
            .ToList();

        return ValueTuple.Create(paginacaoResultadoDTO, repositorioDTOs).Adapt<PaginacaoResultadoDTO<RepositorioDTO>>();
    }

    public void AdicionarFavorito(RepositorioDTO repositorioDTO)
    {
        _repositorioStore.Adicionar(repositorioDTO.Adapt<Repositorio>());
    }

    public List<FavoritoDTO> ListarFavoritos()
    {
        return _repositorioStore
            .ListarTodos()
            .OrderByDescending(RepositorioRelevanciaCalculator.CalcularRelevancia)
            .Select(f => f.Adapt<FavoritoDTO>())
            .ToList();
    }
}