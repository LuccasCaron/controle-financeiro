using ControleFinanceiro.Application.Common;
using MediatR;

namespace ControleFinanceiro.Application.Transacoes.Commands;

public class CriarTransacaoCommand : IRequest<BaseView>
{
    public Guid PessoaId { get; private set; }
    public Guid CategoriaId { get; private set; }
    public Domain.Enums.TipoTransacao Tipo { get; private set; }
    public decimal Valor { get; private set; }
    public DateTime Data { get; private set; }
    public string? Descricao { get; private set; }

    public CriarTransacaoCommand(Guid pessoaId, Guid categoriaId, Domain.Enums.TipoTransacao tipo, decimal valor, DateTime data, string? descricao)
    {
        PessoaId = pessoaId;
        CategoriaId = categoriaId;
        Tipo = tipo;
        Valor = valor;
        Data = data;
        Descricao = descricao;
    }
}
