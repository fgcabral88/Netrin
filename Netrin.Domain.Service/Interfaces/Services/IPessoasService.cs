using Netrin.Application.Dtos.Pessoa;
using Netrin.Application.Responses;

namespace Netrin.Domain.Service.Interfaces.Services
{
    public interface IPessoasService
    {
        Task<PaginacaoResponseBase<ListarPessoasDto>> RetornarPessoaAsync(int page, int pageSize);

        Task<ResponseBase<ListarPessoasDto>> RetornarPessoaIdAsync(Guid id);

        Task<ResponseBase<ListarPessoasDto>> AdicionarPesssoaAsync(CriarPessoasDto criarPessoaDto);

        Task<ResponseBase<ListarPessoasDto>> EditarPessoaAsync(EditarPessoasDto editarPessoaDto);

        Task<ResponseBase<ListarPessoasDto>> DeletarPessoaAsync(Guid Id);
    }
}
