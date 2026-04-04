using System.Text.Json;
using Application.DTOs;
using Application.Interfaces;
using Application.Options;
using Domain.Entities;
using Infrastructure.Clients.GitHub.Response;
using Mapster;
using Microsoft.Extensions.Options;

namespace Infrastructure.Clients.GitHub;

public sealed class GitHubHttpClient : IGitHubHttpClient
{
    private readonly HttpClient _httpClient;

    public GitHubHttpClient(HttpClient httpClient, IOptions<HttpClientOptions> httpClientOptions)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(httpClientOptions.Value.GitHubUri);
        _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("backend-technical-test");
    }

    public async Task<List<RepositorioDTO>> ListarRepositoriosDoUsuario(string usuario)
    {
        var response = await _httpClient.GetAsync($"users/{Uri.EscapeDataString(usuario)}/repos");
        
        response.EnsureSuccessStatusCode();

        var jsonResponse = await response.Content.ReadAsStringAsync();
        
        var repositorios = JsonSerializer.Deserialize<PageResponse<RepositoryResponse>>(jsonResponse);

        var repositorioDTOs = repositorios?.Items
            .Select(r => r.Adapt<RepositorioDTO>()).ToList() ?? new List<RepositorioDTO>();

        return repositorioDTOs;
    }

    public async Task<PaginacaoResultadoDTO<Repositorio>> BuscarRepositoriosPeloNome(string nome, int pagina, int tamanhoPagina)
    {
        var response = await _httpClient.GetAsync($"search/repositories?q={Uri.EscapeDataString(nome)}&page={pagina}&per_page={tamanhoPagina}");
        
        response.EnsureSuccessStatusCode();

        var jsonResponse = await response.Content.ReadAsStringAsync();
        
        var repositorios = JsonSerializer.Deserialize<PageResponse<RepositoryResponse>>(jsonResponse);

        var items = repositorios?.Items
            .Select(r => r.Adapt<Repositorio>())
            .ToList() ?? new List<Repositorio>();

        return new PaginacaoResultadoDTO<Repositorio>
        {
            TotalItens = repositorios?.QuantidadeTotal ?? 0,
            Pagina = pagina,
            TamanhoPagina = tamanhoPagina,
            Itens = items
        };
    }
}
