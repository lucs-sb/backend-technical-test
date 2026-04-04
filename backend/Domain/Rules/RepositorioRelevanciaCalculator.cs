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