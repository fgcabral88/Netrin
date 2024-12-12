namespace Netrin.Application.Responses
{
    public class ResponseBase<Entity>
    {
        public bool Sucesso { get; set; }
        public string Mensagem { get; set; }
        public Entity? Dados { get; set; }

        public ResponseBase(bool sucesso, string mensagem, Entity? dados)
        {
            Sucesso = sucesso;
            Mensagem = mensagem;
            Dados = dados;
        }
    }
}
