namespace Application.DTOs;

public sealed record RepositorioDTO(
    string Nome, 
    string HtmlUrl, 
    int QuantidadeEstrelas, 
    int QuantidadeForks, 
    int QuantidadeObservadores
);