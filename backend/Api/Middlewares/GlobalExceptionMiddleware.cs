using Microsoft.AspNetCore.Mvc;

namespace Api.Middlewares;

public sealed class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IHostEnvironment _environment;

    public GlobalExceptionMiddleware(
        RequestDelegate next,
        IHostEnvironment environment)
    {
        _next = next;
        _environment = environment;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex, _environment);
        }
    }

    private static async Task HandleExceptionAsync(
        HttpContext context,
        Exception exception,
        IHostEnvironment environment)
    {
        var (statusCode, title, detail) = exception switch
        {
            ArgumentException ex => (
                StatusCodes.Status400BadRequest,
                "Requisição inválida",
                ex.Message
            ),

            KeyNotFoundException ex => (
                StatusCodes.Status404NotFound,
                "Recurso não encontrado",
                ex.Message
            ),

            _ => (
                StatusCodes.Status500InternalServerError,
                "Erro interno no servidor",
                environment.IsDevelopment() ? exception.Message : "Ocorreu um erro inesperado."
            )
        };

        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/problem+json";

        var problem = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = detail,
            Instance = context.Request.Path
        };

        await context.Response.WriteAsJsonAsync(problem);
    }
}