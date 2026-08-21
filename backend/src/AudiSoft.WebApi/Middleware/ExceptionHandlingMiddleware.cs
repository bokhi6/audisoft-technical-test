using AudiSoft.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace AudiSoft.WebApi.Middleware;

public static partial class ExceptionHandlingLog
{
    [LoggerMessage(Level = LogLevel.Error, Message = "Error no controlado procesando {Path}")]
    public static partial void ErrorNoControlado(ILogger logger, Exception exception, string path);
}

public class ExceptionHandlingMiddleware
{
    private const int SqlErrorViolacionForeignKey = 547;

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
            await ManejarExcepcionAsync(context, ex);
        }
    }

    private async Task ManejarExcepcionAsync(HttpContext context, Exception exception)
    {
        int status;
        string titulo;
        string detalle;

        switch (exception)
        {
            case ValidationAppException:
                status = StatusCodes.Status400BadRequest;
                titulo = "Error de validación";
                detalle = exception.Message;
                break;
            case AutenticacionException:
                status = StatusCodes.Status401Unauthorized;
                titulo = "No autorizado";
                detalle = exception.Message;
                break;
            case NotFoundException:
                status = StatusCodes.Status404NotFound;
                titulo = "Recurso no encontrado";
                detalle = exception.Message;
                break;
            case ConflictoDeIntegridadException:
                status = StatusCodes.Status409Conflict;
                titulo = "Conflicto de integridad referencial";
                detalle = exception.Message;
                break;
            // Red de seguridad: si una violación de FK (error 547 de SQL Server) llega
            // hasta aquí sin haber sido detectada por la validación de negocio previa,
            // se traduce igualmente a un 409 amigable en vez de un 500 crudo.
            case DbUpdateException dbEx when dbEx.InnerException is SqlException sqlEx
                                              && sqlEx.Number == SqlErrorViolacionForeignKey:
                status = StatusCodes.Status409Conflict;
                titulo = "Conflicto de integridad referencial";
                detalle = "No se puede completar la operación porque el registro tiene datos relacionados.";
                break;
            default:
                status = StatusCodes.Status500InternalServerError;
                titulo = "Error interno del servidor";
                detalle = "Ocurrió un error inesperado. Intente nuevamente más tarde.";
                break;
        }

        if (status == StatusCodes.Status500InternalServerError)
        {
            ExceptionHandlingLog.ErrorNoControlado(_logger, exception, context.Request.Path);
        }

        var problemDetails = new ProblemDetails
        {
            Type = $"https://httpstatuses.com/{status}",
            Title = titulo,
            Status = status,
            Detail = detalle
        };

        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = status;
        await context.Response.WriteAsJsonAsync(problemDetails);
    }
}
