using ControleFinanceiro.Application.Common;

namespace ControleFinanceiro.Application.Pessoas.Queries.Views;

public class ListarPessoasView : BaseView
{
    public PagedResponse<PessoaView> Pessoas { get; private set; }

    public ListarPessoasView(PagedResponse<PessoaView> pessoas)
        : base("Pessoas listadas com sucesso.", 200)
    {
        Pessoas = pessoas;
    }
}

public class PessoaView
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Cpf { get; set; } = string.Empty;
    public DateTime DataNascimento { get; set; }
    public int Idade { get; set; }
}
