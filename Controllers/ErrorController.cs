using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;

namespace ModuloMVC.Controllers
{
  [AllowAnonymous]
    public class ErrorController : Controller
    {
        private readonly ILogger<ErrorController> _logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            _logger = logger;
        }

[Route("Error/{statusCode}")]
    public IActionResult HttpStatusCodeHandler(int statusCode)
    {
        // Captura informações da rota original que deu erro
        var statusCodeResult = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();

        switch (statusCode)
        {
            case 404:
                _logger.LogWarning($"404: O caminho {statusCodeResult?.OriginalPath} não foi encontrado.");
                return View("NotFound");
            
            case 403:
                _logger.LogWarning($"403: Acesso negado para {statusCodeResult?.OriginalPath}.");
                return View("AccessDenied");
                
            default:
                return View("Error");
        }
    }

    [Route("Error/500")]
    public IActionResult ExceptionHandler()
    {
        // Captura os detalhes da exceção que "explodiu"
        var exceptionDetails = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

        // SOLID: Delegamos o log para a infraestrutura
        _logger.LogError(exceptionDetails?.Error, $"Erro Crítico em: {exceptionDetails?.Path}");

        // DDD: Em produção, nunca enviamos a Exception para a View por segurança
        return View("ServerError");
    }

    }
}