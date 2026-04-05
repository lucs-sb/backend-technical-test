using Api.Models;
using FluentValidation;

namespace Api.Validators;

public sealed class RepositorioModelValidator : AbstractValidator<RepositorioModel>
{
    public RepositorioModelValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty()
            .WithMessage("O nome do repositório é obrigatório.");

        RuleFor(x => x.HtmlUrl)
            .NotEmpty()
            .WithMessage("A URL do repositório é obrigatória.");

        RuleFor(x => x.QuantidadeEstrelas)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .WithMessage("A quantidade de estrelas é obrigatória.")
            .GreaterThanOrEqualTo(0)
            .WithMessage("A quantidade de estrelas não pode ser negativa.");

        RuleFor(x => x.QuantidadeForks)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .WithMessage("A quantidade de forks é obrigatória.")
            .GreaterThanOrEqualTo(0)
            .WithMessage("A quantidade de forks não pode ser negativa.");

        RuleFor(x => x.QuantidadeObservadores)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .WithMessage("A quantidade de observadores é obrigatória.")
            .GreaterThanOrEqualTo(0)
            .WithMessage("A quantidade de observadores não pode ser negativa.");
    }
}
