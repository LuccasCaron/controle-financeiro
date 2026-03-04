namespace ControleFinanceiro.Api.DTOs.Pessoa;

/// <summary>
/// DTO para atualização de pessoa.
/// </summary>
public class AtualizarPessoaDto
{
    /// <summary>
    /// Novo nome da pessoa.
    /// </summary>
    public string Nome { get; set; } = string.Empty;

    /// <summary>
    /// Nova data de nascimento da pessoa.
    /// </summary>
    public DateTime DataNascimento { get; set; }
}
