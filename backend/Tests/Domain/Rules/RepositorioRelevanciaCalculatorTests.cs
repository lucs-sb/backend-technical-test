using Tests.TestBuilders;
using Domain.Entities;
using Domain.Rules;

namespace Tests.Domain.Rules;

[TestFixture]
public sealed class RepositorioRelevanciaCalculatorTests
{
    [Test]
    public void CalcularRelevancia_QuandoMetricasValidas_RetornaMediaPonderadaEsperada()
    {
        var repositorio = new RepositorioBuilder()
            .ComMetricas(60, 30, 12)
            .Build();

        var resultado = RepositorioRelevanciaCalculator.CalcularRelevancia(repositorio);

        Assert.That(resultado, Is.EqualTo(42));
    }

    [Test]
    public void CalcularRelevancia_QuandoMetricasZeradas_RetornaZero()
    {
        var repositorio = new RepositorioBuilder()
            .ComMetricas(0, 0, 0)
            .Build();

        var resultado = RepositorioRelevanciaCalculator.CalcularRelevancia(repositorio);

        Assert.That(resultado, Is.EqualTo(0));
    }

    [Test]
    public void CalcularRelevancia_QuandoChamadoMaisDeUmaVez_RetornaMesmoResultado()
    {
        var repositorio = new RepositorioBuilder()
            .ComMetricas(1200, 800, 400)
            .Build();

        var primeiroResultado = RepositorioRelevanciaCalculator.CalcularRelevancia(repositorio);
        var segundoResultado = RepositorioRelevanciaCalculator.CalcularRelevancia(repositorio);

        Assert.That(segundoResultado, Is.EqualTo(primeiroResultado));
    }

    [Test]
    public void CalcularRelevancia_QuandoQuantidadeDeEstrelasIgualQuantidadeDeForks_RetornaScoreMaiorParaEstrelas()
    {
        var repositorioComEstrelas = new RepositorioBuilder().ComQuantidadeEstrelas(10).Build();
        var repositorioComForks = new RepositorioBuilder().ComQuantidadeForks(10).Build();

        var scoreComEstrelas = RepositorioRelevanciaCalculator.CalcularRelevancia(repositorioComEstrelas);
        var scoreComForks = RepositorioRelevanciaCalculator.CalcularRelevancia(repositorioComForks);

        Assert.That(scoreComEstrelas, Is.GreaterThan(scoreComForks));
    }

    [Test]
    public void CalcularRelevancia_QuandoQuantidadeDeForksIgualQuantidadeDeWatchers_RetornaScoreMaiorParaForks()
    {
        var repositorioComForks = new RepositorioBuilder().ComQuantidadeForks(10).Build();
        var repositorioComWatchers = new RepositorioBuilder().ComQuantidadeObservadores(10).Build();

        var scoreComForks = RepositorioRelevanciaCalculator.CalcularRelevancia(repositorioComForks);
        var scoreComWatchers = RepositorioRelevanciaCalculator.CalcularRelevancia(repositorioComWatchers);

        Assert.That(scoreComForks, Is.GreaterThan(scoreComWatchers));
    }
}
