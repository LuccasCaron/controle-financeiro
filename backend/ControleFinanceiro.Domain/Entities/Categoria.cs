using ControleFinanceiro.Domain.Common;
using ControleFinanceiro.Domain.Enums;
using ControleFinanceiro.Domain.Exceptions;
using ControleFinanceiro.Domain.ValueObjects;

namespace ControleFinanceiro.Domain.Entities;

public class Categoria : BaseEntity
{
    public Nome Nome { get; private set; } = null!;
    public Descricao Descricao { get; private set; } = null!;
    public FinalidadeCategoria Finalidade { get; private set; }

    private Categoria()
    {
    }

    public Categoria(Nome nome, Descricao descricao, FinalidadeCategoria finalidade)
    {
        Nome = nome;
        Descricao = descricao;
        Finalidade = finalidade;
    }

    public void AtualizarNome(Nome novoNome)
    {
        Nome = novoNome;
    }

    public void AtualizarDescricao(Descricao novaDescricao)
    {
        Descricao = novaDescricao;
    }

    public void AtualizarFinalidade(FinalidadeCategoria novaFinalidade)
    {
        Finalidade = novaFinalidade;
    }

    public void ValidarTipoTransacaoPermitido(TipoTransacao tipoTransacao)
    {
        var podeUsar = Finalidade switch
        {
            FinalidadeCategoria.Receita => tipoTransacao == TipoTransacao.Receita,
            FinalidadeCategoria.Despesa => tipoTransacao == TipoTransacao.Despesa,
            FinalidadeCategoria.Ambas => true,
            _ => false
        };

        if (!podeUsar)
        {
            var tipoTransacaoTexto = tipoTransacao == TipoTransacao.Receita ? "receita" : "despesa";
            var finalidadeTexto = Finalidade switch
            {
                FinalidadeCategoria.Receita => "receitas",
                FinalidadeCategoria.Despesa => "despesas",
                _ => "ambas"
            };

            throw new DomainException(
                $"A categoria '{Nome.Valor}' é exclusiva para {finalidadeTexto} e não pode ser usada para {tipoTransacaoTexto}.");
        }
    }
}
