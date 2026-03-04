using System.Text.RegularExpressions;
using FluentValidation;

namespace ControleFinanceiro.Application.Pessoas.Commands.Validations;

public class CriarPessoaCommandValidation : AbstractValidator<CriarPessoaCommand>
{
    private static readonly Regex CpfRegex = new(@"^\d{11}$", RegexOptions.Compiled);

    public CriarPessoaCommandValidation()
    {
        RuleFor(x => x.Nome)
            .NotEmpty()
            .WithMessage("O nome é obrigatório.")
            .MaximumLength(100)
            .WithMessage("O nome não pode ter mais de 100 caracteres.");

        RuleFor(x => x.Cpf)
            .NotEmpty()
            .WithMessage("O CPF é obrigatório.")
            .Must(BeValidCpfFormat)
            .WithMessage("O CPF deve conter exatamente 11 dígitos numéricos.");

        RuleFor(x => x.DataNascimento)
            .NotEmpty()
            .WithMessage("A data de nascimento é obrigatória.")
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("A data de nascimento não pode ser no futuro.");
    }

    private static bool BeValidCpfFormat(string? cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf))
            return false;

        var cpfLimpo = Regex.Replace(cpf, @"[^\d]", string.Empty);
        return CpfRegex.IsMatch(cpfLimpo);
    }
}
