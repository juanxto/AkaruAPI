using System.Net;
using System.Text.Json;
using Akaru.Domain.Exceptions;

namespace Akaru.API.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
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
            await TratarExcecaoAsync(context, ex);
        }
    }

    private async Task TratarExcecaoAsync(HttpContext context, Exception exception)
    {
        _logger.LogError(exception, "Erro não tratado: {Message}", exception.Message);

        var (status, mensagem) = exception switch
        {
            NotFoundException => (HttpStatusCode.NotFound, exception.Message),
            AcessoNegadoException => (HttpStatusCode.Forbidden, exception.Message),
            DomainException => (HttpStatusCode.BadRequest, exception.Message),
            _ => (HttpStatusCode.InternalServerError, "Ocorreu um erro interno no servidor.")
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)status;

        var payload = new
        {
            erro = true,
            mensagem,
            status = (int)status,
            timestamp = DateTime.UtcNow
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(payload));
    }
}
