namespace Netrin.Application.Responses
{
    public class PaginacaoResponseBase<Entity> : ResponseBase<IEnumerable<Entity>>
    {
        public int ContagemTotal { get; set; }

        public PaginacaoResponseBase(bool sucesso, string mensagem, IEnumerable<Entity> dados, int totalCount)
            : base(sucesso, mensagem, dados)
        {
            ContagemTotal = totalCount;
        }
    }
}
