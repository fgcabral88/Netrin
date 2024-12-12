using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;
using System.Diagnostics;

namespace Netrin.Api.Presentation.Filters
{
    /// <summary>
    /// Filtro para registrar logs antes e depois da execução de ações do controlador.
    /// </summary>
    public class AcaoFilter : IActionFilter
    {
        private readonly Stopwatch _stopwatch;

        public AcaoFilter()
        {
            _stopwatch = new Stopwatch();
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _stopwatch.Start();

            var nomeAcao = context.ActionDescriptor.DisplayName ?? "Ação desconhecida";
            var pessoa = context.HttpContext.User.Identity?.Name ?? "Pessoa não autenticada";

            Log.Information("Iniciando a execução da ação: {NomeAcao} | Pessoa: {Pessoa}", nomeAcao, pessoa);
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            _stopwatch.Stop();

            var nomeAcao = context.ActionDescriptor.DisplayName ?? "Ação desconhecida";
            var milissegundos = _stopwatch.ElapsedMilliseconds;

            if (context.Exception == null)
            {
                Log.Information("Ação concluída: {NomeAcao} | Tempo de execução: {Milissegundos} ms", nomeAcao, milissegundos);
            }
            else
            {
                Log.Error(context.Exception, "Erro durante a execução da ação: {NomeAcao} | Tempo de execução: {Milissegundos} ms", nomeAcao, milissegundos);
            }
        }
    }
}
