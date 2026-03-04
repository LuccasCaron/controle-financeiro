using ControleFinanceiro.Application.Pessoas.Commands;
using FluentValidation;

namespace ControleFinanceiro.Application.Pessoas.Commands.Validations;

/// <summary>
/// Validador para o comando de atualização de pessoa usando FluentValidation.
/// </summary>
public class AtualizarPessoaCommandValidation : AbstractValidator<AtualizarPessoaCommand>
{
    /// <summary>
    /// Construtor que define as regras de validação.
    /// </summary>
    public AtualizarPessoaCommandValidation()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("O identificador da pessoa é obrigatório.");

        RuleFor(x => x.Nome)
            .NotEmpty()
            .WithMessage("O nome é obrigatório.")
            .MaximumLength(100)
            .WithMessage("O nome não pode ter mais de 100 caracteres.");

        RuleFor(x => x.DataNascimento)
            .NotEmpty()
            .WithMessage("A data de nascimento é obrigatória.")
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("A data de nascimento não pode ser no futuro.")
            .Must(data => data >= DateTime.UtcNow.AddYears(-150))
            .WithMessage($"A data de nascimento não pode ser anterior a {DateTime.UtcNow.AddYears(-150):dd/MM/yyyy}.");
    }
}
