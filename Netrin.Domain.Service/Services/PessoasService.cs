using AutoMapper;
using Netrin.Application.Dtos.Pessoa;
using Netrin.Application.Responses;
using Netrin.Domain.Service.Interfaces.Respository;
using Netrin.Domain.Service.Interfaces.Services;
using Serilog;

namespace Netrin.Application.Services
{
    public class PessoasService : IPessoasService
    {
        private readonly IMapper _mapper;
        private readonly IPessoasRepository _pessoaRepository;

        public PessoasService(IMapper mapper, IPessoasRepository pessoaRepository)
        {
            _mapper = mapper;
            _pessoaRepository = pessoaRepository;
        }

        public async Task<ResponseBase<IEnumerable<ListarPessoasDto>>> RetornarPessoaAsync()
        {
            try
            {
                // Busca os dados no repositório:
                var pessoasResponse = await _pessoaRepository.RetornarPessoaRespositorioAsync();

                // Verifica se a operação foi bem-sucedida e se há dados:
                if (!pessoasResponse.Sucesso || pessoasResponse.Dados is null || !pessoasResponse.Dados.Any())
                {
                    Log.Warning("Nao foram encontradas pessoas cadastradas no banco de dados.");
                    return new ResponseBase<IEnumerable<ListarPessoasDto>>(sucesso: false, mensagem: "Não foram encontradas pessoas cadastradas no banco de dados.", dados: null);
                }

                // Mapeia os dados para o Dto:
                var pessoas = _mapper.Map<List<ListarPessoasDto>>(pessoasResponse.Dados);

                Log.Information($"Pessoas retornadas com sucesso. Total: {pessoas.Count}");
                return new ResponseBase<IEnumerable<ListarPessoasDto>>(sucesso: true, mensagem: "Pessoas retornadas com sucesso.", dados: pessoas);
            }
            catch (Exception ex)
            {
                Log.Error($"Erro ao retornar pessoas: {ex.Message}");
                return new ResponseBase<IEnumerable<ListarPessoasDto>>(sucesso: false, mensagem: "Erro interno.", dados: null);
            }
        }

        public async Task<ResponseBase<ListarPessoasDto>> RetornarPessoaIdAsync(Guid id)
        {
            try
            {
                // Busca o dado no repositório:
                var pessoaIdResponse = await _pessoaRepository.RetornarPessoaIdRespositorioAsync(id); 

                // Verifica se a operação foi bem-sucedida e se há dados:
                if (!pessoaIdResponse.Sucesso || pessoaIdResponse.Dados is null)
                {
                    Log.Warning($"Pessoa com id: '{id}' nao encontrada na base de dados.");
                    return new ResponseBase<ListarPessoasDto>(sucesso: false, mensagem: $"Pessoa com id: '{id}' não encontrada na base de dados.", dados: null);
                }

                // Mapeia os dados para o Dto:
                var pessoaId = _mapper.Map<ListarPessoasDto>(pessoaIdResponse.Dados);

                Log.Information($"Pessoa com id: '{id}' retornada com sucesso.");
                return new ResponseBase<ListarPessoasDto>(sucesso: true, mensagem: "Pessoa retornada com sucesso.", dados: pessoaId);
            }
            catch (Exception ex)
            {
                Log.Error($"Erro ao retornar pessoa com id: '{id}': {ex.Message}");
                return new ResponseBase<ListarPessoasDto>(sucesso: false, mensagem: "Erro interno.", dados: null);
            }
        }

        public Task<ResponseBase<ListarPessoasDto>> AdicionarPesssoaAsync(CriarPessoasDto criarPessoaDto)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseBase<ListarPessoasDto>> EditarPessoaAsync(EditarPessoasDto editarPessoaDto)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseBase<ListarPessoasDto>> DeletarPessoaAsync(Guid Id)
        {
            throw new NotImplementedException();
        }
    }
}
