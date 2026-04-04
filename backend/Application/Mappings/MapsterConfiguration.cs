using Application.DTOs;
using Domain.Entities;
using Mapster;

namespace Application.Mappings;

public static class MapsterConfiguration
{
    public static void Register()
    {
        TypeAdapterConfig<(PaginacaoResultadoDTO<Repositorio>, List<RepositorioDTO>), PaginacaoResultadoDTO<RepositorioDTO>>
            .NewConfig()
            .Map(dest => dest.TotalItens, src => src.Item1.TotalItens)
            .Map(dest => dest.Pagina, src => src.Item1.Pagina)
            .Map(dest => dest.TamanhoPagina, src => src.Item1.TamanhoPagina)
            .Map(dest => dest.Itens, src => src.Item2);
    }
}
