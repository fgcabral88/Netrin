namespace Netrin.Application.Responses
{
    public class ResponseBase<Entity>
    {
        public Entity? Dados { get; set; }
        public string Mensagem { get; set; }
        public bool Sucesso { get; set; }

        public ResponseBase() { }

        public ResponseBase(Entity? dados, string mensagem, bool sucesso)
        {
            Dados = dados;
            Mensagem = mensagem;
            Sucesso = sucesso;
        }
    }
}
