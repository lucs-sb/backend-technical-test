using System.Text.Json;
using Application.DTOs;
using Application.Interfaces;
using Application.Options;
using Infrastructure.Integrations.Response;
using Microsoft.Extensions.Options;

namespace Infrastructure.Integrations;

public sealed class GitHubIntegration : IGitHubIntegration
{
    private readonly HttpClient _httpClient;

    public GitHubIntegration(HttpClient httpClient, IOptions<IntegrationOptions> integrationOptions)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(integrationOptions.Value.GitHubUri);
    }

    public async Task<List<RepositorioDTO>> ListarRepositoriosDoUsuario(string usuario)
    {
        var response = await _httpClient.GetAsync($"users/{Uri.EscapeDataString(usuario)}/repos");
        
        response.EnsureSuccessStatusCode();

        await using var contentStream = await response.Content.ReadAsStreamAsync();
        
        var repositorios = await JsonSerializer.DeserializeAsync<List<GitHubRepositoryResponse>>(contentStream);

        var repositorioDTOs = repositorios?
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

        await using var contentStream = await response.Content.ReadAsStreamAsync();
        
        var repositorios = await JsonSerializer.DeserializeAsync<List<GitHubRepositoryResponse>>(contentStream);

        var repositorioDTOs = repositorios?
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
