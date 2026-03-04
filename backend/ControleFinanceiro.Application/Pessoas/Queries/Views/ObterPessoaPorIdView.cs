using ControleFinanceiro.Application.Common;

namespace ControleFinanceiro.Application.Pessoas.Queries.Views;

public class ObterPessoaPorIdView : BaseView
{
    public PessoaView? Pessoa { get; private set; }

    public ObterPessoaPorIdView(PessoaView? pessoa)
        : base(pessoa != null ? "Pessoa encontrada com sucesso." : "Pessoa não encontrada.", pessoa != null ? 200 : 404)
    {
        Pessoa = pessoa;
    }
}
