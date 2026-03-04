using ControleFinanceiro.Application.Common;

namespace ControleFinanceiro.Application.Transacoes.Commands.Views;

public class CriarTransacaoView : BaseView
{
    public Guid Id { get; private set; }

    public CriarTransacaoView(Guid id)
        : base("Transação criada com sucesso.", 201)
    {
        Id = id;
    }
}
