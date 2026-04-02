using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
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

    public async Task<List<RepositorioDTO>> ListarRepositoriosDoUsuario(string usuario)
    {
        return await _gitHubHttpClient.ListarRepositoriosDoUsuario(usuario);
    }

    public async Task<List<RepositorioDTO>> BuscarRepositoriosPeloNome(string nome)
    {
        return await _gitHubHttpClient.BuscarRepositoriosPeloNome(nome);
    }

    public void AdicionarFavorito(RepositorioDTO repositorioDTO)
    {
        _repositorioStore.Adicionar(repositorioDTO.Adapt<Repositorio>());
    }

    public List<FavoritoDTO> ListarFavoritos()
    {
        return _repositorioStore
            .ListarTodos()
            .OrderByDescending(CalcularRelevancia)
            .Select(f => f.Adapt<FavoritoDTO>())
            .ToList();
    }

    private static double CalcularRelevancia(Repositorio repositorio)
    {
        const double pesoEstrelas = 3;
        const double pesoForks = 2;
        const double pesoWatchers = 1;

        /*
         A relevância é calculada por média ponderada.
         Estrelas pesam mais porque indicam aprovação geral do repositório.
         Forks vêm em seguida por mostrarem uso mais ativo, como estudo ou contribuição.
         Watchers têm menor peso porque indicam interesse, mas não necessariamente uso.
        */
        var somaPesos = pesoEstrelas + pesoForks + pesoWatchers;

        return ((repositorio.QuantidadeEstrelas * pesoEstrelas)
            + (repositorio.QuantidadeForks * pesoForks)
            + (repositorio.QuantidadeObservadores * pesoWatchers)) / somaPesos;
    }
}