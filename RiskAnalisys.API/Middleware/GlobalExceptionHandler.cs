using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace RiskAnalisys.API.Middleware
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "Ocorreu um erro não tratado: {Message}", exception.Message);

            var (statusCode, title, message) = exception switch
             {
                 InvalidOperationException or { InnerException: InvalidOperationException } => (StatusCodes.Status400BadRequest, "Operação Inválida", exception.Message),
                 UnauthorizedAccessException or { InnerException: UnauthorizedAccessException } => (StatusCodes.Status401Unauthorized, "Não Autorizado", "Sem aautorizacao para acessar recurso"),
                 _ => (StatusCodes.Status500InternalServerError, "Erro Interno", "Mais detalhes dos erros nos logs")
             };

            httpContext.Response.StatusCode = statusCode;

            await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Detail = message
            }, cancellationToken);

            return true;
        }
    }
}
