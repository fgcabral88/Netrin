using Netrin.Application.Dtos.Pessoa;
using Netrin.Application.Responses;

namespace Netrin.Domain.Service.Interfaces.Services
{
    public interface IPessoaService
    {
        Task<ResponseBase<IEnumerable<ListarPessoaDto>>> RetornarPessoaAsync();

        Task<ResponseBase<ListarPessoaDto>> RetornarPessoaIdAsync(Guid id);

        Task<ResponseBase<ListarPessoaDto>> AdicionarPesssoaAsync(CriarPessoaDto criarPessoaDto);

        Task<ResponseBase<ListarPessoaDto>> EditarPessoaAsync(EditarPessoaDto editarPessoaDto);

        Task<ResponseBase<ListarPessoaDto>> DeletarPessoaAsync(Guid Id);
    }
}
