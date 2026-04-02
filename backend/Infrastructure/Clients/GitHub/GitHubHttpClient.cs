using System.Text.Json;
using Application.DTOs;
using Application.Interfaces;
using Application.Options;
using Infrastructure.Clients.GitHub.Response;
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
            .Select(r => new RepositorioDTO(
                r.Nome, 
                r.HtmlUrl, 
                r.QuantidadeEstrelas, 
                r.QuantidadeForks, 
                r.QuantidadeObservadores)
            ).ToList() ?? new List<RepositorioDTO>();

        return repositorioDTOs;
    }

    public async Task<List<RepositorioDTO>> BuscarRepositoriosPeloNome(string nome)
    {
        var response = await _httpClient.GetAsync($"search/repositories?q={Uri.EscapeDataString(nome)}");
        
        response.EnsureSuccessStatusCode();

        var jsonResponse = await response.Content.ReadAsStringAsync();
        
        var repositorios = JsonSerializer.Deserialize<PageResponse<RepositoryResponse>>(jsonResponse);

        var repositorioDTOs = repositorios?.Items
            .Select(r => new RepositorioDTO(
                r.Nome, 
                r.HtmlUrl, 
                r.QuantidadeEstrelas, 
                r.QuantidadeForks, 
                r.QuantidadeObservadores)
            ).ToList() ?? new List<RepositorioDTO>();

        return repositorioDTOs;
    }
}
