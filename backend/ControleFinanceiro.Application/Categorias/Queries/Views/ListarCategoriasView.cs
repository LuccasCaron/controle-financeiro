using ControleFinanceiro.Application.Common;

namespace ControleFinanceiro.Application.Categorias.Queries.Views;

public class ListarCategoriasView : BaseView
{
    public PagedResponse<CategoriaView> Categorias { get; private set; }

    public ListarCategoriasView(PagedResponse<CategoriaView> categorias)
        : base("Categorias listadas com sucesso.", 200)
    {
        Categorias = categorias;
    }
}

public class CategoriaView
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public Domain.Enums.FinalidadeCategoria Finalidade { get; set; }
}
