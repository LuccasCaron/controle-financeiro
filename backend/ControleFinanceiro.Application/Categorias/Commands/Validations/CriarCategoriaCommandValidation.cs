using ControleFinanceiro.Application.Categorias.Commands;
using FluentValidation;

namespace ControleFinanceiro.Application.Categorias.Commands.Validations;

public class CriarCategoriaCommandValidation : AbstractValidator<CriarCategoriaCommand>
{
    public CriarCategoriaCommandValidation()
    {
        RuleFor(x => x.Nome)
            .NotEmpty()
            .WithMessage("O nome é obrigatório.")
            .MaximumLength(100)
            .WithMessage("O nome não pode ter mais de 100 caracteres.");

        RuleFor(x => x.Descricao)
            .MaximumLength(500)
            .WithMessage("A descrição não pode ter mais de 500 caracteres.");

        RuleFor(x => x.Finalidade)
            .IsInEnum()
            .WithMessage("A finalidade deve ser Receita, Despesa ou Ambas.");
    }
}
