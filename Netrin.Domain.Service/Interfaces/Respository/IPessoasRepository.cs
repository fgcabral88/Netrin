using Netrin.Application.Dtos.Pessoa;
using Netrin.Application.Responses;

namespace Netrin.Domain.Service.Interfaces.Respository
{
    public interface IPessoasRepository
    {
        Task<PaginacaoResponseBase<ListarPessoasDto>> RetornarPessoaRespositorioAsync(int page, int pageSize);

        Task<ResponseBase<ListarPessoasDto>> RetornarPessoaIdRespositorioAsync(Guid id);

        Task<ResponseBase<ListarPessoasDto>> AdicionarPesssoaRepositorioAsync(CriarPessoasDto criarPessoaDto);

        Task<ResponseBase<ListarPessoasDto>> EditarPessoaRepositorioAsync(EditarPessoasDto editarPessoaDto);

        Task<ResponseBase<ListarPessoasDto>> DeletarPessoaRepositorioAsync(Guid Id);
    }
}
