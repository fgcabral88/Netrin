using AutoMapper;
using Netrin.Application.Dtos.Pessoa;
using Netrin.Application.Responses;
using Netrin.Domain.Service.Interfaces.Respository;
using Netrin.Domain.Service.Interfaces.Services;
using Serilog;

namespace Netrin.Application.Services
{
    public class PessoaService : IPessoaService
    {
        private readonly IMapper _mapper;
        private readonly IPessoaRepository _pessoaRepository;

        public PessoaService(IMapper mapper, IPessoaRepository pessoaRepository)
        {
            _mapper = mapper;
            _pessoaRepository = pessoaRepository;
        }

        public async Task<ResponseBase<IEnumerable<ListarPessoaDto>>> RetornarPessoaAsync()
        {
            try
            {
                // Busca os dados no repositório:
                var pessoasResponse = await _pessoaRepository.RetornarPessoaRespositorioAsync();

                // Verifica se a operação foi bem-sucedida e se há dados:
                if (!pessoasResponse.Sucesso || pessoasResponse.Dados is null || !pessoasResponse.Dados.Any())
                {
                    Log.Warning("Nao foram encontradas pessoas cadastradas no banco de dados.");
                    return new ResponseBase<IEnumerable<ListarPessoaDto>>(sucesso: false, mensagem: "Não foram encontradas pessoas cadastradas no banco de dados.", dados: null);
                }

                // Mapeia os dados para o Dto:
                var pessoas = _mapper.Map<List<ListarPessoaDto>>(pessoasResponse.Dados);

                Log.Information($"Pessoas retornadas com sucesso. Total: {pessoas.Count}");
                return new ResponseBase<IEnumerable<ListarPessoaDto>>(sucesso: true, mensagem: "Pessoas retornadas com sucesso.", dados: pessoas);
            }
            catch (Exception ex)
            {
                Log.Error($"Erro ao retornar pessoas: {ex.Message}");
                return new ResponseBase<IEnumerable<ListarPessoaDto>>(sucesso: false, mensagem: "Erro interno.", dados: null);
            }
        }

        public async Task<ResponseBase<ListarPessoaDto>> RetornarPessoaIdAsync(Guid id)
        {
            try
            {
                // Busca o dado no repositório:
                var pessoaIdResponse = await _pessoaRepository.RetornarPessoaIdRespositorioAsync(id); 

                // Verifica se a operação foi bem-sucedida e se há dados:
                if (!pessoaIdResponse.Sucesso || pessoaIdResponse.Dados is null)
                {
                    Log.Warning($"Pessoa com id: '{id}' nao encontrada na base de dados.");
                    return new ResponseBase<ListarPessoaDto>(sucesso: false, mensagem: $"Pessoa com id: '{id}' nao encontrada na base de dados.", dados: null);
                }

                // Mapeia os dados para o Dto:
                var pessoaId = _mapper.Map<ListarPessoaDto>(pessoaIdResponse.Dados);

                Log.Information($"Pessoa com id: '{id}' retornada com sucesso.");
                return new ResponseBase<ListarPessoaDto>(sucesso: true, mensagem: "Pessoa retornada com sucesso.", dados: pessoaId);
            }
            catch (Exception ex)
            {
                Log.Error($"Erro ao retornar pessoa com id: '{id}': {ex.Message}");
                return new ResponseBase<ListarPessoaDto>(sucesso: false, mensagem: "Erro interno.", dados: null);
            }
        }

        public Task<ResponseBase<ListarPessoaDto>> AdicionarPesssoaAsync(CriarPessoaDto criarPessoaDto)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseBase<ListarPessoaDto>> EditarPessoaAsync(EditarPessoaDto editarPessoaDto)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseBase<ListarPessoaDto>> DeletarPessoaAsync(Guid Id)
        {
            throw new NotImplementedException();
        }
    }
}
