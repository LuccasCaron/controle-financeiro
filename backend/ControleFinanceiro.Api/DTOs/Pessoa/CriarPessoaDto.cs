namespace ControleFinanceiro.Api.DTOs.Pessoa;

public class CriarPessoaDto
{
    public string Nome { get; set; } = string.Empty;
    public string Cpf { get; set; } = string.Empty;
    public DateTime DataNascimento { get; set; }
}
