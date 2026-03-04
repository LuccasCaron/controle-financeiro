using ControleFinanceiro.Application.Common;
using ControleFinanceiro.Domain.Exceptions;
using FluentValidation;
using System.Net;
using System.Text.Json;

namespace ControleFinanceiro.Api.Middleware;

public class GlobalExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

    public GlobalExceptionHandlerMiddleware(
        RequestDelegate next,
        ILogger<GlobalExceptionHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var response = context.Response;
        response.ContentType = "application/json";

        ErrorView errorView;

        switch (exception)
        {
            case DomainException domainException:
                _logger.LogWarning(domainException, "Erro de domínio: {Message}", domainException.Message);
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                errorView = new ErrorView(domainException.Message, (int)HttpStatusCode.BadRequest);
                break;

            case ValidationException validationException:
                _logger.LogWarning(validationException, "Erro de validação");
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                var errors = validationException.Errors
                    .Select(e => e.ErrorMessage)
                    .ToList();
                var errorMessage = string.Join(" ", errors);
                errorView = new ErrorView(errorMessage, (int)HttpStatusCode.BadRequest);
                break;

            default:
                _logger.LogError(exception, "Erro não tratado: {Message}", exception.Message);
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                errorView = new ErrorView(
                    "Ocorreu um erro interno no servidor. Tente novamente mais tarde.",
                    (int)HttpStatusCode.InternalServerError);
                break;
        }

        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        var jsonResponse = JsonSerializer.Serialize(errorView, jsonOptions);
        await response.WriteAsync(jsonResponse);
    }
}
