using ControleFinanceiro.Application.Common;

namespace ControleFinanceiro.Application.Transacoes.Queries.Views;

public class ListarTransacoesView : BaseView
{
    public PagedResponse<TransacaoView> Transacoes { get; private set; }

    public ListarTransacoesView(PagedResponse<TransacaoView> transacoes)
        : base("Transações listadas com sucesso.", 200)
    {
        Transacoes = transacoes;
    }
}

public class TransacaoView
{
    public Guid Id { get; set; }
    public Guid PessoaId { get; set; }
    public string PessoaNome { get; set; } = string.Empty;
    public Guid CategoriaId { get; set; }
    public string CategoriaNome { get; set; } = string.Empty;
    public Domain.Enums.TipoTransacao Tipo { get; set; }
    public decimal Valor { get; set; }
    public DateTime Data { get; set; }
    public string? Descricao { get; set; }
}
