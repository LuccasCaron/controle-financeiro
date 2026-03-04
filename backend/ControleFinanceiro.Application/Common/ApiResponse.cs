namespace ControleFinanceiro.Application.Common;

public abstract class ApiResponse
{
    public bool Success => StatusCode >= 200 && StatusCode < 300;

    public string Message { get; protected set; } = string.Empty;

    public int StatusCode { get; protected set; }

    protected ApiResponse(string message, int statusCode)
    {
        Message = message;
        StatusCode = statusCode;
    }
}
