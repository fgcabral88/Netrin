using Dapper;
using Microsoft.Data.SqlClient;
using Netrin.Application.Dtos.Pessoa;
using Netrin.Application.Responses;
using Netrin.Domain.Service.Interfaces.Respository;
using Netrin.Infraestructure.Data.Context;
using Serilog;

namespace Netrin.Infraestructure.Repositories
{
    public class PessoaRepository : IPessoaRepository, IDisposable
    {
        private readonly SqlDbContext _dbContext;
        private bool _disposed = false;

        public PessoaRepository(SqlDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ResponseBase<IEnumerable<ListarPessoaDto>>> RetornarPessoaRespositorioAsync()
        {
            try
            {
                // Consulta sql todas as pessoas:
                const string query = "SELECT * FROM Pessoa";

                // Abre a conexão com o banco de dados:
                using var conexao = _dbContext.CriarConexao(); 
                conexao.Open();

                // Recupera todas as pessoas no banco de dados:
                var pessoas = await conexao.QueryAsync<ListarPessoaDto>(query);

                // Valida se as pessoas foram encontradas:
                if (!pessoas.Any())
                {
                    return new ResponseBase<IEnumerable<ListarPessoaDto>>(sucesso: false, mensagem: "Não foram encontrados pessoas no banco de dados.", dados: null);
                }

                // Retonar as pessoas encontradas:
                return new ResponseBase<IEnumerable<ListarPessoaDto>>(sucesso: true, mensagem: "Pessoas recuperadas com sucesso do banco de dados.", dados: pessoas);
            }
            catch (SqlException ex)
            {
                Log.Error(ex.Message, ex);
                return new ResponseBase<IEnumerable<ListarPessoaDto>>(sucesso: false, mensagem: ex.Message, dados: null);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                return new ResponseBase<IEnumerable<ListarPessoaDto>>(sucesso: false, mensagem: ex.Message, dados: null);
            }
        }

        public async Task<ResponseBase<ListarPessoaDto>> RetornarPessoaIdRespositorioAsync(Guid id)
        {
            // Consulta a Pessoa pelo Id:
            const string query = "SELECT * FROM Pessoa WHERE Id = @Id";

            try
            {
                // Abre a conexão com o banco de dados:
                using var conexao = _dbContext.CriarConexao(); 
                conexao.Open();

                // Recupera a Pessoa no banco de dados:
                var pessoaId = await conexao.QueryFirstOrDefaultAsync<ListarPessoaDto>(query, new { Id = id });

                // Valida se a Pessoa foi encontrada:
                if (pessoaId is null)
                {
                    return new ResponseBase<ListarPessoaDto>(sucesso: false, mensagem: $"Pessoa com id {id} não encontrada no banco de dados.", dados: null);
                }

                return new ResponseBase<ListarPessoaDto>(sucesso: true, mensagem: "Pessoa recuperada com sucesso.", dados: pessoaId);
            }
            catch (SqlException ex)
            {
                Log.Error(ex.Message, ex);
                return new ResponseBase<ListarPessoaDto>(sucesso: false, mensagem: ex.Message, dados: null);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                return new ResponseBase<ListarPessoaDto>(sucesso: false, mensagem: ex.Message, dados: null);
            }
        }

        public Task<ResponseBase<ListarPessoaDto>> AdicionarPesssoaRepositorioAsync(CriarPessoaDto criarPessoaDto)
        {
          throw new NotImplementedException();
        }

        public Task<ResponseBase<ListarPessoaDto>> EditarPessoaRepositorioAsync(EditarPessoaDto editarPessoaDto)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseBase<ListarPessoaDto>> DeletarPessoaRepositorioAsync(Guid Id)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                if (_dbContext != null)
                {
                    _dbContext.CriarConexao().Dispose(); 
                }

                _disposed = true;
            }
        }
    }
}
