namespace ControleFinanceiro.Application.Common;

public abstract class BaseView : ApiResponse
{
    protected BaseView(string message, int statusCode)
        : base(message, statusCode)
    {
    }
}
