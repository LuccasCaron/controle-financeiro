using ControleFinanceiro.Application.Common;

namespace ControleFinanceiro.Application.Pessoas.Commands.Views;

public class CriarPessoaView : BaseView
{
    public Guid Id { get; private set; }

    public CriarPessoaView(Guid id)
        : base("Pessoa criada com sucesso.", 201)
    {
        Id = id;
    }
}
