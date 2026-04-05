using System.Net.Http;
using Application.DTOs;
using Application.Interfaces;
using Application.Services;
using Tests.TestBuilders;
using Domain.Entities;
using Moq;

namespace Tests.Application.Services;

[TestFixture]
public sealed class RepositorioServiceTests
{
    private Mock<IGitHubHttpClient> _gitHubHttpClientMock = null!;
    private Mock<IRepositorioStore> _repositorioStoreMock = null!;
    private RepositorioService _sut = null!;

    [SetUp]
    public void SetUp()
    {
        _gitHubHttpClientMock = new Mock<IGitHubHttpClient>(MockBehavior.Strict);
        _repositorioStoreMock = new Mock<IRepositorioStore>(MockBehavior.Strict);
        _sut = new RepositorioService(_gitHubHttpClientMock.Object, _repositorioStoreMock.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _gitHubHttpClientMock.VerifyNoOtherCalls();
        _repositorioStoreMock.VerifyNoOtherCalls();
    }

    [Test]
    public async Task BuscarRepositoriosPeloNome_QuandoProviderRetornaItensComScoresDiferentes_RetornaItensOrdenadosPorRelevanciaEPreservaPaginacao()
    {
        var repositorios = new[]
        {
            new RepositorioBuilder().ComNome("watchers").ComQuantidadeObservadores(30).Build(),
            new RepositorioBuilder().ComNome("stars").ComQuantidadeEstrelas(30).Build(),
            new RepositorioBuilder().ComNome("forks").ComQuantidadeForks(30).Build()
        };

        var paginacaoProvider = new PaginacaoResultadoBuilder<Repositorio>()
            .ComTotalItens(50)
            .ComPagina(2)
            .ComTamanhoPagina(3)
            .ComItens(repositorios)
            .Build();

        _gitHubHttpClientMock
            .Setup(client => client.BuscarRepositoriosPeloNome("openai", 2, 3))
            .ReturnsAsync(paginacaoProvider);

        var resultado = await _sut.BuscarRepositoriosPeloNome("openai", 2, 3);

        Assert.Multiple(() =>
        {
            Assert.That(resultado.TotalItens, Is.EqualTo(50));
            Assert.That(resultado.Pagina, Is.EqualTo(2));
            Assert.That(resultado.TamanhoPagina, Is.EqualTo(3));
            Assert.That(
                ProjetarRepositorios(resultado.Itens),
                Is.EqualTo(ProjetarRepositorios(new[]
                {
                    new RepositorioDtoBuilder().ComNome("stars").ComMetricas(30, 0, 0).Build(),
                    new RepositorioDtoBuilder().ComNome("forks").ComMetricas(0, 30, 0).Build(),
                    new RepositorioDtoBuilder().ComNome("watchers").ComMetricas(0, 0, 30).Build()
                })));
        });

        _gitHubHttpClientMock.Verify(client => client.BuscarRepositoriosPeloNome("openai", 2, 3), Times.Once);
    }

    [Test]
    public async Task BuscarRepositoriosPeloNome_QuandoProviderRetornaListaVazia_RetornaPaginacaoVazia()
    {
        var paginacaoProvider = new PaginacaoResultadoBuilder<Repositorio>()
            .ComTotalItens(0)
            .ComPagina(1)
            .ComTamanhoPagina(10)
            .ComItens(Array.Empty<Repositorio>())
            .Build();

        _gitHubHttpClientMock
            .Setup(client => client.BuscarRepositoriosPeloNome("openai", 1, 10))
            .ReturnsAsync(paginacaoProvider);

        var resultado = await _sut.BuscarRepositoriosPeloNome("openai", 1, 10);

        Assert.Multiple(() =>
        {
            Assert.That(resultado.TotalItens, Is.EqualTo(0));
            Assert.That(resultado.Itens, Is.Empty);
            Assert.That(resultado.Pagina, Is.EqualTo(1));
            Assert.That(resultado.TamanhoPagina, Is.EqualTo(10));
        });

        _gitHubHttpClientMock.Verify(client => client.BuscarRepositoriosPeloNome("openai", 1, 10), Times.Once);
    }

    [Test]
    public void BuscarRepositoriosPeloNome_QuandoProviderLancaHttpRequestException_PropagaMesmaMensagemDeErro()
    {
        const string mensagemEsperada = "Falha na integração com o GitHub.";

        _gitHubHttpClientMock
            .Setup(client => client.BuscarRepositoriosPeloNome("openai", 1, 10))
            .ThrowsAsync(new HttpRequestException(mensagemEsperada));

        var excecao = Assert.ThrowsAsync<HttpRequestException>(
            async () => await _sut.BuscarRepositoriosPeloNome("openai", 1, 10));

        Assert.That(excecao!.Message, Is.EqualTo(mensagemEsperada));
        _gitHubHttpClientMock.Verify(client => client.BuscarRepositoriosPeloNome("openai", 1, 10), Times.Once);
    }

    [Test]
    public void AdicionarFavorito_QuandoRecebeDtoValido_AdicionaRepositorioMapeadoNoStore()
    {
        var dto = new RepositorioDtoBuilder()
            .ComNome("backend-technical-test")
            .ComMetricas(10, 5, 3)
            .Build();

        _repositorioStoreMock
            .Setup(store => store.Adicionar(It.Is<Repositorio>(repositorio =>
                repositorio.Nome == dto.Nome
                && repositorio.HtmlUrl == dto.HtmlUrl
                && repositorio.QuantidadeEstrelas == dto.QuantidadeEstrelas
                && repositorio.QuantidadeForks == dto.QuantidadeForks
                && repositorio.QuantidadeObservadores == dto.QuantidadeObservadores)));

        _sut.AdicionarFavorito(dto);

        _repositorioStoreMock.Verify(store => store.Adicionar(It.Is<Repositorio>(repositorio =>
            repositorio.Nome == dto.Nome
            && repositorio.HtmlUrl == dto.HtmlUrl
            && repositorio.QuantidadeEstrelas == dto.QuantidadeEstrelas
            && repositorio.QuantidadeForks == dto.QuantidadeForks
            && repositorio.QuantidadeObservadores == dto.QuantidadeObservadores)), Times.Once);
    }

    [Test]
    public void ListarFavoritos_QuandoStoreRetornaItensForaDaOrdemDeRelevancia_RetornaFavoritosOrdenadosPorRelevancia()
    {
        var repositorios = new[]
        {
            new RepositorioBuilder().ComNome("watchers").ComQuantidadeObservadores(30).Build(),
            new RepositorioBuilder().ComNome("stars").ComQuantidadeEstrelas(30).Build(),
            new RepositorioBuilder().ComNome("forks").ComQuantidadeForks(30).Build()
        };

        _repositorioStoreMock
            .Setup(store => store.ListarTodos())
            .Returns(repositorios);

        var resultado = _sut.ListarFavoritos();

        Assert.That(resultado.Select(item => item.Nome), Is.EqualTo(new[] { "stars", "forks", "watchers" }));
        _repositorioStoreMock.Verify(store => store.ListarTodos(), Times.Once);
    }

    [Test]
    public void ListarFavoritos_QuandoStoreRetornaListaVazia_RetornaListaVazia()
    {
        _repositorioStoreMock
            .Setup(store => store.ListarTodos())
            .Returns(Array.Empty<Repositorio>());

        var resultado = _sut.ListarFavoritos();

        Assert.That(resultado, Is.Empty);
        _repositorioStoreMock.Verify(store => store.ListarTodos(), Times.Once);
    }

    [Test]
    public void RemoverFavorito_QuandoRepositorioExiste_RemoveFavoritoERetornaTrue()
    {
        var id = Guid.NewGuid();
        var repositorio = new RepositorioBuilder()
            .ComNome("backend-technical-test")
            .Build();

        repositorio.Id = id;

        _repositorioStoreMock
            .Setup(store => store.BuscarPorId(id))
            .Returns(repositorio);

        _repositorioStoreMock
            .Setup(store => store.RemoverPeloId(id));

        var resultado = _sut.RemoverFavorito(id);

        Assert.That(resultado, Is.True);
        _repositorioStoreMock.Verify(store => store.BuscarPorId(id), Times.Once);
        _repositorioStoreMock.Verify(store => store.RemoverPeloId(id), Times.Once);
    }

    [Test]
    public void RemoverFavorito_QuandoRepositorioNaoExiste_RetornaFalseSemRemover()
    {
        var id = Guid.NewGuid();

        _repositorioStoreMock
            .Setup(store => store.BuscarPorId(id))
            .Returns((Repositorio?)null);

        var resultado = _sut.RemoverFavorito(id);

        Assert.That(resultado, Is.False);
        _repositorioStoreMock.Verify(store => store.BuscarPorId(id), Times.Once);
        _repositorioStoreMock.Verify(store => store.RemoverPeloId(id), Times.Never);
    }

    private static List<(string Nome, string HtmlUrl, int QuantidadeEstrelas, int QuantidadeForks, int QuantidadeObservadores)>
        ProjetarRepositorios(IEnumerable<RepositorioDTO> repositorios)
    {
        return repositorios
            .Select(repositorio => (
                repositorio.Nome,
                repositorio.HtmlUrl,
                repositorio.QuantidadeEstrelas,
                repositorio.QuantidadeForks,
                repositorio.QuantidadeObservadores))
            .ToList();
    }
}
