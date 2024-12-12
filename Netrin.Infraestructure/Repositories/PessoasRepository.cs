using Dapper;
using Microsoft.Data.SqlClient;
using Netrin.Application.Dtos.Pessoa;
using Netrin.Application.Responses;
using Netrin.Domain.Service.Interfaces.Respository;
using Netrin.Infraestructure.Data.Context;
using Serilog;

namespace Netrin.Infraestructure.Repositories
{
    public class PessoasRepository : IPessoasRepository, IDisposable
    {
        private readonly SqlDbContext _dbContext;
        private bool _disposed = false;

        public PessoasRepository(SqlDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PaginacaoResponseBase<ListarPessoasDto>> RetornarPessoaRespositorioAsync(int page, int pageSize)
        {
            try
            {
                // Calcular o OFFSET:
                int offset = (page - 1) * pageSize;

                // Consulta SQL com paginação:
                const string query = @"SELECT * FROM Pessoas ORDER BY DataCadastro ASC OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY; SELECT COUNT(*) FROM Pessoas;";

                // Abre a conexão com o banco de dados:
                using var conexao = _dbContext.CriarConexao();
                conexao.Open();

                using var multi = await conexao.QueryMultipleAsync(query, new { Offset = offset, pageSize = pageSize });

                var pessoas = await multi.ReadAsync<ListarPessoasDto>();

                var contagemTotal = await multi.ReadSingleAsync<int>();

                if (!pessoas.Any())
                {
                    return new PaginacaoResponseBase<ListarPessoasDto>(sucesso: false, mensagem: "Nenhuma pessoa encontrada no banco de dados.", dados: null, totalCount: 0);
                }

                return new PaginacaoResponseBase<ListarPessoasDto>(sucesso: true, mensagem: "Pessoas recuperadas com sucesso do banco de dados.", dados: pessoas.ToList(), totalCount: contagemTotal);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                return new PaginacaoResponseBase<ListarPessoasDto>(sucesso: false, mensagem: ex.Message, dados: null, totalCount: 0);
            }
        }

        public async Task<ResponseBase<ListarPessoasDto>> RetornarPessoaIdRespositorioAsync(Guid id)
        {
            // Consulta a Pessoa pelo Id:
            const string query = "SELECT * FROM Pessoas WHERE Id = @Id";

            try
            {
                // Abre a conexão com o banco de dados:
                using var conexao = _dbContext.CriarConexao(); 
                conexao.Open();

                // Recupera a Pessoa no banco de dados:
                var pessoaId = await conexao.QueryFirstOrDefaultAsync<ListarPessoasDto>(query, new { Id = id });

                // Valida se a Pessoa foi encontrada:
                if (pessoaId is null)
                {
                    return new ResponseBase<ListarPessoasDto>(sucesso: false, mensagem: $"Pessoa com id {id} não encontrada no banco de dados.", dados: null);
                }

                return new ResponseBase<ListarPessoasDto>(sucesso: true, mensagem: "Pessoa recuperada com sucesso.", dados: pessoaId);
            }
            catch (SqlException ex)
            {
                Log.Error(ex.Message, ex);
                return new ResponseBase<ListarPessoasDto>(sucesso: false, mensagem: ex.Message, dados: null);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                return new ResponseBase<ListarPessoasDto>(sucesso: false, mensagem: ex.Message, dados: null);
            }
        }

        public async Task<ResponseBase<ListarPessoasDto>> AdicionarPesssoaRepositorioAsync(CriarPessoasDto criarPessoaDto)
        {
            // Normaliza o campo Estado para maiúsculas:
            criarPessoaDto.Estado = criarPessoaDto.Estado!.ToUpperInvariant();

            // Cadastra a Pessoa no banco de dados:
            const string query = @"INSERT INTO Pessoas (Nome, Sobrenome, DataNascimento, Email, Sexo, Telefone, Cpf, Cidade, Estado) OUTPUT INSERTED.Id
                         VALUES (@Nome, @Sobrenome, @DataNascimento, @Email, @Sexo, @Telefone, @Cpf, @Cidade, @Estado);";

            try
            {
                // Abre conexão com o banco de dados:
                using var conexao = _dbContext.CriarConexao();
                conexao.Open();

                // Consulta para verificar se a Pessoa já existe no banco de dados:
                const string queryVerificacao = @"SELECT COUNT(1) FROM Pessoas WHERE Nome = @Nome AND Sobrenome = @Sobrenome AND Cpf = @Cpf";

                var existePessoa = await conexao.ExecuteScalarAsync<int>(queryVerificacao, new { criarPessoaDto.Nome, criarPessoaDto.Sobrenome, criarPessoaDto.Cpf });

                if (existePessoa > 0)
                {
                    return new ResponseBase<ListarPessoasDto>(sucesso: false, mensagem: "Pessoa ja existe no banco de dados.", dados: null);
                }

                // Inicia uma transação:
                using var transacao = conexao.BeginTransaction();

                // Retorna o Id recém-inserido:
                var pessoaId = await conexao.ExecuteScalarAsync<Guid>(query, criarPessoaDto, transaction: transacao);

                // Valida se a Pessoa foi adicionada ao banco de dados:
                if (pessoaId == Guid.Empty)
                {
                    transacao.Rollback();

                    Log.Warning("Nenhuma Pessoa foi adicionada ao banco de dados.");
                    return new ResponseBase<ListarPessoasDto>(sucesso: false, mensagem: "Nenhuma Pessoa foi adicionada ao banco de dados.", dados: null);
                }

                // Consulta a Pessoa recém-inserida:
                const string querySelect = @"SELECT Id, Nome, Sobrenome, DataNascimento, Email, Sexo, Telefone, Cpf, Cidade, Estado, DataCadastro, DataAtualizacao, Ativo FROM Pessoas WHERE Id = @Id";

                // Recupera a Pessoa no banco de dados:
                var pessoas = (await conexao.QueryAsync<ListarPessoasDto>(querySelect, new { Id = pessoaId }, transaction: transacao)).ToList();

                transacao.Commit();

                return new ResponseBase<ListarPessoasDto>(sucesso: true, mensagem: "Pessoa cadastrada com sucesso.", dados: pessoas.FirstOrDefault()!);
            }
            catch (SqlException ex)
            {
                Log.Error(ex.Message, ex);
                return new ResponseBase<ListarPessoasDto>(sucesso: false, mensagem: ex.Message, dados: null);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                return new ResponseBase<ListarPessoasDto>(sucesso: false, mensagem: ex.Message, dados: null);
            }
        }

        public async Task<ResponseBase<ListarPessoasDto>> EditarPessoaRepositorioAsync(EditarPessoasDto editarPessoaDto)
        {
            try
            {
                // Query para atualizar Pessoa:
                const string query = @"UPDATE Pessoas SET Nome = @Nome, Sobrenome = @Sobrenome, DataNascimento = @DataNascimento, Email = @Email, Telefone = @Telefone, 
                           Cidade = @Cidade, Estado = @Estado WHERE Id = @Id; SELECT * FROM Pessoas WHERE Id = @Id;";

                // Abre conexão com o banco de dados:
                using var conexao = _dbContext.CriarConexao(); 
                conexao.Open();

                // Inicia uma transação:
                using var transacao = conexao.BeginTransaction();

                // Executa a query de atualização e retorna a Pessoa editada:
                var pessoaEditada = await conexao.QueryAsync<ListarPessoasDto>(query, editarPessoaDto, transaction: transacao);

                // Verifica se a Pessoa foi encontrada:
                if (!pessoaEditada.Any())
                {
                    transacao.Rollback();

                    Log.Warning("Nenhuma Pessoa foi editada.");
                    return new ResponseBase<ListarPessoasDto>(sucesso: false, mensagem: "Nenhuma Pessoa foi editada.", dados: null);
                }

                // Confirma a transação
                transacao.Commit();

                Log.Information("Pessoa editada com sucesso.");
                return new ResponseBase<ListarPessoasDto>(sucesso: true, mensagem: "Pessoa editada com sucesso.", dados: pessoaEditada.FirstOrDefault());
            }
            catch (SqlException ex)
            {
                Log.Error(ex.Message, ex);
                return new ResponseBase<ListarPessoasDto>(sucesso: false, mensagem: ex.Message, dados: null);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                return new ResponseBase<ListarPessoasDto>(sucesso: false, mensagem: ex.Message, dados: null);
            }
        }

        public async Task<ResponseBase<ListarPessoasDto>> DeletarPessoaRepositorioAsync(Guid Id)
        {
            try
            {
                // Query para recuperar os dados da Pessoa antes de deletar:
                const string querySelect = @"SELECT Id, Nome, Sobrenome, DataNascimento, Email, Sexo, Telefone, Cpf, Cidade, Estado, DataCadastro, DataAtualizacao, Ativo 
                       FROM Pessoas WHERE Id = @Id";

                // Query para deletar a Pessoa:
                const string queryDelete = "DELETE FROM Pessoas WHERE Id = @Id;";

                // Abre uma conexão com o banco de dados:
                using var conexao = _dbContext.CriarConexao();
                conexao.Open();

                // Inicia uma transação:
                using var transacao = conexao.BeginTransaction();

                // Recupera os dados da Pessoa:
                var pessoaDelete = await conexao.QueryFirstOrDefaultAsync<ListarPessoasDto>(querySelect, new { Id = Id }, transaction: transacao);

                // Verifica se a Pessoa foi encontrada:
                if (pessoaDelete is null)
                {
                    transacao.Rollback();

                    Log.Warning("Pessoa não encontrada no banco de dados.");
                    return new ResponseBase<ListarPessoasDto>(sucesso: false, mensagem: "Pessoa não encontrada no banco de dados.", dados: null);
                }

                // Executa a query de exclusão:
                var linhasAfetadas = await conexao.ExecuteAsync(queryDelete, new { Id = Id }, transaction: transacao);

                // Valida se a exclusão foi realizada:
                if (linhasAfetadas == 0)
                {
                    transacao.Rollback();

                    Log.Warning("Nenhuma Pessoa foi deletada.");
                    return new ResponseBase<ListarPessoasDto>(sucesso: false, mensagem: "Nenhuma Pessoa foi deletada.", dados: null);
                }

                // Confirma a transação:
                transacao.Commit();

                Log.Information("Pessoa deletada com sucesso.");
                return new ResponseBase<ListarPessoasDto>(sucesso: true, mensagem: "Pessoa deletada com sucesso.", dados: pessoaDelete);
            }
            catch (SqlException ex)
            {
                Log.Error(ex.Message, ex);
                return new ResponseBase<ListarPessoasDto>(sucesso: false, mensagem: ex.Message, dados: null);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                 return new ResponseBase<ListarPessoasDto>(sucesso: false, mensagem: ex.Message, dados: null);
            }
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
