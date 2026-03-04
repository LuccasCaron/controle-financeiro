using ControleFinanceiro.Domain.Common;
using ControleFinanceiro.Domain.Enums;
using ControleFinanceiro.Domain.Exceptions;
using ControleFinanceiro.Domain.ValueObjects;

namespace ControleFinanceiro.Domain.Entities;

public class Transacao : BaseEntity
{
    public Guid PessoaId { get; private set; }
    public Pessoa Pessoa { get; private set; } = null!;
    public Guid CategoriaId { get; private set; }
    public Categoria Categoria { get; private set; } = null!;
    public TipoTransacao Tipo { get; private set; }
    public Dinheiro Valor { get; private set; } = null!;
    public DateTime Data { get; private set; }
    public Descricao? Descricao { get; private set; }

    private Transacao()
    {
    }

    public Transacao(
        Pessoa pessoa,
        Categoria categoria,
        TipoTransacao tipo,
        Dinheiro valor,
        DateTime data,
        Descricao? descricao = null)
    {
        pessoa.ValidarTipoTransacaoPermitido(tipo);
        categoria.ValidarTipoTransacaoPermitido(tipo);

        if (data > DateTime.UtcNow)
        {
            throw new DomainException("A data da transação não pode ser no futuro.");
        }

        PessoaId = pessoa.Id;
        Pessoa = pessoa;
        CategoriaId = categoria.Id;
        Categoria = categoria;
        Tipo = tipo;
        Valor = valor;
        Data = data;
        Descricao = descricao;
    }

    public void AtualizarValor(Dinheiro novoValor)
    {
        Valor = novoValor;
    }

    public void AtualizarData(DateTime novaData)
    {
        if (novaData > DateTime.UtcNow)
        {
            throw new DomainException("A data da transação não pode ser no futuro.");
        }

        Data = novaData;
    }

    public void AtualizarDescricao(Descricao? novaDescricao)
    {
        Descricao = novaDescricao;
    }

    public void AtualizarCategoria(Categoria novaCategoria)
    {
        novaCategoria.ValidarTipoTransacaoPermitido(Tipo);
        CategoriaId = novaCategoria.Id;
        Categoria = novaCategoria;
    }
}
