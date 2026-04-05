using Domain.Entities;

namespace Tests.TestBuilders;

public sealed class RepositorioBuilder
{
    private string _nome = "repositorio";
    private string _htmlUrl = "https://github.com/openai/repositorio";
    private int _quantidadeEstrelas;
    private int _quantidadeForks;
    private int _quantidadeObservadores;

    public RepositorioBuilder ComNome(string nome)
    {
        _nome = nome;
        _htmlUrl = $"https://github.com/openai/{nome}";
        return this;
    }

    public RepositorioBuilder ComHtmlUrl(string htmlUrl)
    {
        _htmlUrl = htmlUrl;
        return this;
    }

    public RepositorioBuilder ComQuantidadeEstrelas(int quantidadeEstrelas)
    {
        _quantidadeEstrelas = quantidadeEstrelas;
        return this;
    }

    public RepositorioBuilder ComQuantidadeForks(int quantidadeForks)
    {
        _quantidadeForks = quantidadeForks;
        return this;
    }

    public RepositorioBuilder ComQuantidadeObservadores(int quantidadeObservadores)
    {
        _quantidadeObservadores = quantidadeObservadores;
        return this;
    }

    public RepositorioBuilder ComMetricas(int quantidadeEstrelas, int quantidadeForks, int quantidadeObservadores)
    {
        _quantidadeEstrelas = quantidadeEstrelas;
        _quantidadeForks = quantidadeForks;
        _quantidadeObservadores = quantidadeObservadores;
        return this;
    }

    public Repositorio Build()
    {
        return new Repositorio
        {
            Nome = _nome,
            HtmlUrl = _htmlUrl,
            QuantidadeEstrelas = _quantidadeEstrelas,
            QuantidadeForks = _quantidadeForks,
            QuantidadeObservadores = _quantidadeObservadores
        };
    }
}
