using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;

namespace Netrin.Api.Presentation.Filters
{
    /// <summary>
    /// Filtros de Ação - Permitem executar lógica antes ou depois de uma ação do controlador
    /// </summary>
    public class AcaoFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            Log.Information("Executando ação: {ActionName}", context.ActionDescriptor.DisplayName);
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            Log.Information("Ação concluída: {ActionName}", context.ActionDescriptor.DisplayName);
        }
    }
}
