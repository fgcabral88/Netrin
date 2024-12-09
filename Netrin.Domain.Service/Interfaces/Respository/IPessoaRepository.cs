using Netrin.Application.Dtos.Pessoa;
using Netrin.Application.Responses;

namespace Netrin.Domain.Service.Interfaces.Respository
{
    public interface IPessoaRepository
    {
        Task<ResponseBase<IEnumerable<ListarPessoaDto>>> RetornarPessoaRespositorioAsync();

        Task<ResponseBase<ListarPessoaDto>> RetornarPessoaIdRespositorioAsync(Guid id);

        Task<ResponseBase<ListarPessoaDto>> AdicionarPesssoaRepositorioAsync(CriarPessoaDto criarPessoaDto);

        Task<ResponseBase<ListarPessoaDto>> EditarPessoaRepositorioAsync(EditarPessoaDto editarPessoaDto);

        Task<ResponseBase<ListarPessoaDto>> DeletarPessoaRepositorioAsync(Guid Id);
    }
}
