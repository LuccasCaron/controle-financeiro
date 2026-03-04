using ControleFinanceiro.Application.Common;

namespace ControleFinanceiro.Application.Categorias.Commands.Views;

public class CriarCategoriaView : BaseView
{
    public Guid Id { get; private set; }

    public CriarCategoriaView(Guid id)
        : base("Categoria criada com sucesso.", 201)
    {
        Id = id;
    }
}
