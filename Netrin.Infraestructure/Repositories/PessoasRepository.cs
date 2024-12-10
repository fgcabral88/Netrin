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

        public async Task<ResponseBase<IEnumerable<ListarPessoasDto>>> RetornarPessoaRespositorioAsync()
        {
            try
            {
                // Consulta sql todas as pessoas:
                const string query = @"SELECT * FROM Pessoas ORDER BY DataCadastro ASC";

                // Abre a conexão com o banco de dados:
                using var conexao = _dbContext.CriarConexao();
                conexao.Open();

                // Recupera todas as pessoas no banco de dados:
                var pessoas = await conexao.QueryAsync<ListarPessoasDto>(query);

                // Valida se as pessoas foram encontradas:
                if (!pessoas.Any())
                {
                    return new ResponseBase<IEnumerable<ListarPessoasDto>>(sucesso: false, mensagem: "Não foram encontrados pessoas no banco de dados.", dados: null);
                }

                // Retonar as pessoas encontradas:
                return new ResponseBase<IEnumerable<ListarPessoasDto>>(sucesso: true, mensagem: "Pessoas recuperadas com sucesso do banco de dados.", dados: pessoas);
            }
            catch (SqlException ex)
            {
                Log.Error(ex.Message, ex);
                return new ResponseBase<IEnumerable<ListarPessoasDto>>(sucesso: false, mensagem: ex.Message, dados: null);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                return new ResponseBase<IEnumerable<ListarPessoasDto>>(sucesso: false, mensagem: ex.Message, dados: null);
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
            const string query = @"INSERT INTO Pessoas (Nome, Sobrenome, DataNascimento, Email, Sexo, Telefone, Cpf, Cidade, Estado, DataCadastro, DataAtualizacao, Ativo) OUTPUT INSERTED.Id
            VALUES (@Nome, @Sobrenome, @DataNascimento, @Email, @Sexo, @Telefone, @Cpf, @Cidade, @Estado, @DataCadastro, @DataAtualizacao, @Ativo);";

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

                return new ResponseBase<ListarPessoasDto>(sucesso: true, mensagem: "Pessoa cadastrada com sucesso.", dados: pessoas.FirstOrDefault());
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
                const string query = @"UPDATE Pessoas SET Nome = @Nome, Sobrenome = @Sobrenome, DataNascimento = @DataNascimento, Email = @Email, Sexo = @Sexo, Telefone = @Telefone, 
                     Cpf = @Cpf, Cidade = @Cidade, Estado = @Estado, DataCadastro = @DataCadastro, DataAtualizacao = @DataAtualizacao, Ativo = @Ativo WHERE Id = @Id; SELECT * FROM Pessoas 
                     WHERE Id = @Id;";

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

        public Task<ResponseBase<ListarPessoasDto>> DeletarPessoaRepositorioAsync(Guid Id)
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
