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
                Log.Error($"Erro ao retornar pessoas: {ex.Message}", ex);
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
                Log.Error($"Erro ao retornar pessoa com id: '{id}': {ex.Message}", ex);
                return new ResponseBase<ListarPessoasDto>(sucesso: false, mensagem: "Erro interno.", dados: null);
            }
        }

        public async Task<ResponseBase<ListarPessoasDto>> AdicionarPesssoaAsync(CriarPessoasDto criarPessoaDto)
        {
            try
            {
                // Chamada ao repositório para adicionar Pessoa:
                var pessoaAdicionarResponse = await _pessoaRepository.AdicionarPesssoaRepositorioAsync(criarPessoaDto);

                // Verifica o retorno do repositório:
                if (!pessoaAdicionarResponse.Sucesso)
                {
                    Log.Warning("Não foi possível adicionar Pessoa.");
                    return new ResponseBase<ListarPessoasDto>(sucesso: false, mensagem: "Não foi possível adicionar Pessoa.", dados: null);
                }

                // Recupera a Pessoa adicionada:
                var pessoaAdicionada = pessoaAdicionarResponse.Dados;

                // Verifica se a Pessoa foi adicionada:
                if (pessoaAdicionada is null)
                {
                    Log.Warning("Falha ao recuperar a Pessoa recém-cadastrada.");
                    return new ResponseBase<ListarPessoasDto>(sucesso: false, mensagem: "Falha ao recuperar a Pessoa recém-cadastrada.", dados: null);
                }

                Log.Information("Pessoa adicionada com sucesso.");
                return new ResponseBase<ListarPessoasDto>(sucesso: true, mensagem: "Pessoa adicionada com sucesso.", dados: pessoaAdicionada);
            }
            catch (Exception ex)
            {
                Log.Error($"Erro ao adicionar pessoa: {ex.Message}", ex);
                return new ResponseBase<ListarPessoasDto>(sucesso: false, mensagem: "Erro ao adicionar Pessoa.", dados: null);
            }
        }

        public async Task<ResponseBase<ListarPessoasDto>> EditarPessoaAsync(EditarPessoasDto editarPessoaDto)
        {
            try
            {
                // Chamada ao repositório para editar Pessoa:
                var pessoaEditarResponse = await _pessoaRepository.EditarPessoaRepositorioAsync(editarPessoaDto);

                // Verifica o retorno do repositório:
                if (!pessoaEditarResponse.Sucesso)
                {
                    Log.Warning("Não foi possível editar Pessoa.");
                    return new ResponseBase<ListarPessoasDto>(sucesso: false, mensagem: "Não foi possível editar Pessoa.", dados: null);
                }

                // Recupera a Pessoa editada:
                var pessoasEditadas = pessoaEditarResponse.Dados;

                // Verifica se a Pessoa foi editada:
                if (pessoasEditadas is null)
                {
                    Log.Warning("Falha ao recuperar a cerveja recém-editada.");
                    return new ResponseBase<ListarPessoasDto>(sucesso: false, mensagem: "Falha ao recuperar a Pessoa recém-editada.", dados: null);
                }

                Log.Information("Pessoa editada com sucesso.");
                return new ResponseBase<ListarPessoasDto>(sucesso: true, mensagem: "Pessoa editada com sucesso.", dados: pessoasEditadas);
            }
            catch (Exception ex)
            {
                Log.Error($"Erro ao editar a pessoa: {ex.Message}", ex);
                return new ResponseBase<ListarPessoasDto>(sucesso: false, mensagem: "Erro ao editar a pessoa.", dados: null);
            }
        }

        public async Task<ResponseBase<ListarPessoasDto>> DeletarPessoaAsync(Guid Id)
        {
            try
            {
                // Chamada ao repositório para deletar a Pessoa:
                var pessoasDeletarResponse = await _pessoaRepository.DeletarPessoaRepositorioAsync(Id);

                // Verifica o retorno do repositório:
                if (!pessoasDeletarResponse.Sucesso)
                {
                    Log.Warning("Não foi possível deletar a Pessoa.");
                    return new ResponseBase<ListarPessoasDto>(sucesso: false, mensagem: "Não foi possível deletar a Pessoa.", dados: null);
                }

                // Recupera a Pessoa deletada:
                var pessoaDeletada = pessoasDeletarResponse.Dados; 

                // Verifica se a Pessoa foi deletada:
                if (pessoaDeletada is null)
                {
                    Log.Warning("Falha ao recuperar a Pessoa recém-deletada.");
                    return new ResponseBase<ListarPessoasDto>(sucesso: false, mensagem: "Falha ao recuperar a Pessoa recém-deletada.", dados: null);
                }

                Log.Information("Pessoa deletada com sucesso.");
                return new ResponseBase<ListarPessoasDto>(sucesso: true, mensagem: "Pessoa deletada com sucesso.", dados: pessoaDeletada);
            }
            catch (Exception ex)
            {
                Log.Error($"Erro ao deletar a Pessoa: {ex.Message}", ex);
                return new ResponseBase<ListarPessoasDto>(sucesso: false, mensagem: "Erro ao deletar a Pessoa.", dados: null);
            }
        }
    }
}
