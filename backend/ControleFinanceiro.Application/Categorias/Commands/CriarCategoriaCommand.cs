using ControleFinanceiro.Application.Common;
using MediatR;

namespace ControleFinanceiro.Application.Categorias.Commands;

public class CriarCategoriaCommand : IRequest<BaseView>
{
    public string Nome { get; private set; }
    public string? Descricao { get; private set; }
    public Domain.Enums.FinalidadeCategoria Finalidade { get; private set; }

    public CriarCategoriaCommand(string nome, string? descricao, Domain.Enums.FinalidadeCategoria finalidade)
    {
        Nome = nome;
        Descricao = descricao;
        Finalidade = finalidade;
    }
}
