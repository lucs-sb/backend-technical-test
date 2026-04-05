using Domain.Entities;

namespace Domain.Rules;

public static class RepositorioRelevanciaCalculator
{
    public static double CalcularRelevancia(Repositorio repositorio)
    {
        const double pesoEstrelas = 3;
        const double pesoForks = 2;
        const double pesoWatchers = 1;

        /*
         A relevância é calculada usando uma média ponderada com escala logarítmica.

         As métricas públicas do GitHub variam em ordens de grandeza, a escala logarítmica ajuda a equilibrar isso, diminuindo o impacto de valores muito altos, mas ainda preservando a diferença entre os repositórios.

         Em relação aos pesos:
            - Estrelas têm maior peso, pois representam a aprovação geral do repositório.
            - Forks vêm em seguida, indicando uso mais ativo, como estudos ou contribuições.
            - Watchers têm menor peso, pois refletem interesse, mas não necessariamente uso direto.
        */
        var somaPesos = pesoEstrelas + pesoForks + pesoWatchers;

        return ((AplicarEscalaLogaritmica(repositorio.QuantidadeEstrelas) * pesoEstrelas)
            + (AplicarEscalaLogaritmica(repositorio.QuantidadeForks) * pesoForks)
            + (AplicarEscalaLogaritmica(repositorio.QuantidadeObservadores) * pesoWatchers)) / somaPesos;
    }

    private static double AplicarEscalaLogaritmica(int valor) => Math.Log(valor + 1d); 
}
