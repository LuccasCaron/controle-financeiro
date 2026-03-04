namespace ControleFinanceiro.Application.Common;

public class ErrorView : BaseView
{
    public ErrorView(string message, int statusCode)
        : base(message, statusCode)
    {
    }

    public static ErrorView NotFound(string message = "Recurso não encontrado.")
    {
        return new ErrorView(message, 404);
    }

    public static ErrorView BadRequest(string message = "Requisição inválida.")
    {
        return new ErrorView(message, 400);
    }

    public static ErrorView InternalServerError(string message = "Erro interno do servidor.")
    {
        return new ErrorView(message, 500);
    }
}
