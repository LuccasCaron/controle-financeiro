using ControleFinanceiro.Application.Transacoes.Commands;
using FluentValidation;

namespace ControleFinanceiro.Application.Transacoes.Commands.Validations;

public class CriarTransacaoCommandValidation : AbstractValidator<CriarTransacaoCommand>
{
    public CriarTransacaoCommandValidation()
    {
        RuleFor(x => x.PessoaId)
            .NotEmpty()
            .WithMessage("O identificador da pessoa é obrigatório.");

        RuleFor(x => x.CategoriaId)
            .NotEmpty()
            .WithMessage("O identificador da categoria é obrigatório.");

        RuleFor(x => x.Tipo)
            .IsInEnum()
            .WithMessage("O tipo da transação deve ser Receita ou Despesa.");

        RuleFor(x => x.Valor)
            .GreaterThan(0)
            .WithMessage("O valor deve ser maior que zero.");

        RuleFor(x => x.Data)
            .NotEmpty()
            .WithMessage("A data é obrigatória.")
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("A data não pode ser no futuro.");

        RuleFor(x => x.Descricao)
            .MaximumLength(500)
            .WithMessage("A descrição não pode ter mais de 500 caracteres.");
    }
}
