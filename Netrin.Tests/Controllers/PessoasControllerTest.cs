using Microsoft.AspNetCore.Mvc;
using Moq;
using Netrin.Api.Presentation.Controllers;
using Netrin.Application.Dtos.Pessoa;
using Netrin.Application.Responses;
using Netrin.Domain.Core.Enums;
using Netrin.Domain.Service.Interfaces.Services;

namespace Netrin.Tests.Controllers
{
    public class PessoasControllerTest
    {
        private readonly PessoasController _pessoasController;
        private readonly Mock<IPessoasService> _mockPessoasService;

        public PessoasControllerTest()
        {
            _mockPessoasService = new Mock<IPessoasService>();
            _pessoasController = new PessoasController(_mockPessoasService.Object);
        }

        [Fact]
        public async Task RetornarPessoasAsync_Sucesso_RetornaResultadoOk()
        {
            // Arrange
            var pessoas = new List<ListarPessoasDto>
            {
                new ListarPessoasDto
                {
                    Id = Guid.NewGuid(),
                    Nome = "Pessoa Teste",
                    Sobrenome = "Sobrenome Teste",
                    DataNascimento = DateTime.Now,
                    Cpf = "08766339634",
                    Sexo = SexoEnum.Masculino,
                    Telefone = "+5541999999999",
                    Email = "testeunitario@teste.com.br",
                    Ativo = AtivoEnum.Ativo,
                    DataCadastro = DateTime.Now,
                    DataAtualizacao = DateTime.Now,
                    Cidade = "Curitiba",
                    Estado = "PR"
                }
            };

            var resposta = new ResponseBase<IEnumerable<ListarPessoasDto>>(pessoas, "Sucesso", true);

            _mockPessoasService.Setup(x => x.RetornarPessoaAsync())
                .ReturnsAsync(resposta);

            // Act
            var resultado = await _pessoasController.RetornarPessoas();

            // Assert
            var resultadoOk = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal(200, resultadoOk.StatusCode);

            var retornarResultado = Assert.IsType<List<ListarPessoasDto>>(resultadoOk.Value);

            Assert.Equal(pessoas, retornarResultado);
        }

        [Fact]
        public async Task RetornarPessoasAsync_NaoEncontrado_RetornaNotFound()
        {
            // Arrange
            var resposta = new ResponseBase<IEnumerable<ListarPessoasDto>>(null, "Nenhuma Pessoa encontrada", false);

            _mockPessoasService.Setup(s => s.RetornarPessoaAsync()).ReturnsAsync(resposta);

            // Act
            var resultado = await _pessoasController.RetornarPessoas();

            // Assert
            var resultadoNotFound = Assert.IsType<NotFoundObjectResult>(resultado);
            Assert.Equal("Nenhuma Pessoa encontrada", resultadoNotFound.Value);
        }

    }
}