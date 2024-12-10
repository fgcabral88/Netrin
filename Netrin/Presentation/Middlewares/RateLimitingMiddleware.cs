using System.Collections.Concurrent;
using System.Net;

public class RateLimitingMiddleware
{
    private static readonly ConcurrentDictionary<string, (int Count, DateTime Timestamp)> _requests = new();
    private readonly RequestDelegate _proximo;
    private readonly int _limite;
    private readonly TimeSpan _janelaTempo;

    public RateLimitingMiddleware(RequestDelegate proximo, int limite, TimeSpan janelaTempo)
    {
        _proximo = proximo;
        _limite = limite;
        _janelaTempo = janelaTempo;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var chave = context.Connection.RemoteIpAddress!.ToString();
        var agora = DateTime.UtcNow;
        var requestInfo = _requests.GetOrAdd(chave, (0, agora));

        if (requestInfo.Timestamp + _janelaTempo < agora)
        {
            // Redefine a contagem se a janela de tempo tiver passado
            _requests[chave] = (1, agora);
        }
        else
        {
            // Aumentar a contagem
            if (requestInfo.Count >= _limite)
            {
                context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsync("{\"Mensagem\": \"Limite de requisições excedido. Por favor, tente novamente mais tarde\"}");

                return;
            }

            _requests[chave] = (requestInfo.Count + 1, requestInfo.Timestamp);
        }

        await _proximo(context);
    }
}