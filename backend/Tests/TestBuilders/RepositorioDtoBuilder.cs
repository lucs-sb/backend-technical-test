using Application.DTOs;

namespace Tests.TestBuilders;

public sealed class RepositorioDtoBuilder
{
    private string _nome = "repositorio";
    private string _htmlUrl = "https://github.com/openai/repositorio";
    private int _quantidadeEstrelas;
    private int _quantidadeForks;
    private int _quantidadeObservadores;

    public RepositorioDtoBuilder ComNome(string nome)
    {
        _nome = nome;
        _htmlUrl = $"https://github.com/openai/{nome}";
        return this;
    }

    public RepositorioDtoBuilder ComHtmlUrl(string htmlUrl)
    {
        _htmlUrl = htmlUrl;
        return this;
    }

    public RepositorioDtoBuilder ComMetricas(int quantidadeEstrelas, int quantidadeForks, int quantidadeObservadores)
    {
        _quantidadeEstrelas = quantidadeEstrelas;
        _quantidadeForks = quantidadeForks;
        _quantidadeObservadores = quantidadeObservadores;
        return this;
    }

    public RepositorioDTO Build()
    {
        return new RepositorioDTO(
            _nome,
            _htmlUrl,
            _quantidadeEstrelas,
            _quantidadeForks,
            _quantidadeObservadores);
    }
}
