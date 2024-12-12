using Moq;
using Netrin.Api.Presentation.Controllers;
using Netrin.Domain.Service.Interfaces.Services;
using Netrin.Application.Dtos.Pessoa;
using Netrin.Application.Responses;
using Microsoft.AspNetCore.Mvc;

public class PessoasControllerTests
{
    private readonly Mock<IPessoasService> _mockPessoasService;
    private readonly PessoasController _controller;

    public PessoasControllerTests()
    {
        _mockPessoasService = new Mock<IPessoasService>();
        _controller = new PessoasController(_mockPessoasService.Object);
    }

    [Fact]
    public async Task RetornarPessoas_DeveRetornarOk_QuandoServicoRetornarSucesso()
    {
        // Arrange
 
        //Cria um objeto de resposta simulada com sucesso para o servi�o.
        var resposta = new PaginacaoResponseBase<ListarPessoasDto>(true, "Sucesso", new List<ListarPessoasDto> { new ListarPessoasDto() }, 1);

        //Configura o mock do servi�o para retornar a resposta simulada quando o m�todo RetornarPessoaAsync for chamado.
        _mockPessoasService.Setup(s => s.RetornarPessoaAsync(1, 4)).ReturnsAsync(resposta);

        // Act

        //Chama o m�todo do controlador que ser� testado (RetornarPessoas), passando os par�metros necess�rios.
        var resultado = await _controller.RetornarPessoas(1, 4);

        // Assert

        //Verifica se o resultado � do tipo OkObjectResult, o que indica sucesso.
        var resultadoOk = Assert.IsType<OkObjectResult>(resultado);

        //Verifica se o valor retornado dentro do OkObjectResult � igual � resposta simulada.
        Assert.Equal(resposta, resultadoOk.Value);
    }

    [Fact]
    public async Task RetornarPessoas_DeveRetornarNotFound_QuandoServicoNaoRetornarDados()
    {
        // Arrange

        //Cria uma resposta simulada com falha, indicando que nenhum dado foi encontrado.
        var resposta = new PaginacaoResponseBase<ListarPessoasDto>(false, "Nenhum dado encontrado", null!, 1);

        //Configura o mock do servi�o para retornar a resposta simulada quando o m�todo RetornarPessoaAsync for chamado com os par�metros 1 e 4.
        _mockPessoasService.Setup(s => s.RetornarPessoaAsync(1, 4)).ReturnsAsync(resposta);

        // Act

        //Chama o m�todo do controlador (RetornarPessoas) que ser� testado, passando os par�metros 1 e 4.
        var resultado = await _controller.RetornarPessoas(1, 4);

        // Assert

        //Verifica se o resultado retornado � do tipo NotFoundObjectResult, o que indica que nenhum dado foi encontrado.
        var restauldoNotFound = Assert.IsType<NotFoundObjectResult>(resultado);

        //Verifica se a mensagem retornada dentro do NotFoundObjectResult corresponde � mensagem da resposta simulada.
        Assert.Equal(resposta.Mensagem, restauldoNotFound.Value);
    }

    [Fact]
    public async Task RetornarPessoasId_DeveRetornarOk_QuandoServicoRetornarSucesso()
    {
        // Arrange

        //Cria uma resposta simulada com sucesso para o servi�o, com um objeto ListarPessoasDto e status 'true' indicando sucesso.
        var resposta = new ResponseBase<ListarPessoasDto>(true, "Sucesso", new ListarPessoasDto());
        //Cria um Id �nico usando Guid.NewGuid() para simular a busca de uma pessoa espec�fica.
        var id = Guid.NewGuid();

        //Configura o mock do servi�o (_mockPessoasService) para retornar a resposta simulada quando o m�todo RetornarPessoaIdAsync for chamado com o Id gerado.
        _mockPessoasService.Setup(s => s.RetornarPessoaIdAsync(id)).ReturnsAsync(resposta);

        // Act

        //Chama o m�todo do controlador (RetornarPessoasId) que ser� testado, passando o ID gerado como par�metro.
        var resultado = await _controller.RetornarPessoasId(id);

        // Assert

        //Verifica se o resultado retornado � do tipo OkObjectResult, indicando sucesso e que o status HTTP retornado � 200 (OK).
        var resultadoOk = Assert.IsType<OkObjectResult>(resultado);

        //Verifica se o valor retornado dentro do OkObjectResult � igual � resposta simulada, garantindo que os dados retornados est�o corretos.
        Assert.Equal(resposta, resultadoOk.Value);
    }

    [Fact]
    public async Task RetornarPessoasId_DeveRetornarBadRequest_QuandoIdEstiverVazio()
    {
        // Act

        //Chama o m�todo do controlador (RetornarPessoasId) que ser� testado, passando um ID vazio (Guid.Empty).
        var resultado = await _controller.RetornarPessoasId(Guid.Empty);

        // Assert

        //Verifica se o resultado retornado � do tipo BadRequestObjectResult, indicando que o ID vazio causou uma resposta de erro (400).
        var resultadoBadRequest = Assert.IsType<BadRequestObjectResult>(resultado);

        //Verifica se a mensagem de erro retornada dentro do BadRequestObjectResult corresponde � mensagem esperada ("Id informado � inv�lido.").
        Assert.Equal("Id informado � inv�lido.", resultadoBadRequest.Value);
    }

    [Fact]
    public async Task AdicionarPessoas_ShouldReturnOk_WhenServiceReturnsSuccess()
    {
        // Arrange
        var response = new ResponseBase<ListarPessoasDto>
        {
            Sucesso = true,
            Dados = new ListarPessoasDto(),
            Mensagem = "Success"
        };

        var criarPessoasDto = new CriarPessoasDto();
        _mockPessoasService.Setup(s => s.AdicionarPesssoaAsync(criarPessoasDto)).ReturnsAsync(response);

        // Act
        var result = await _controller.AdicionarPessoas(criarPessoasDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(response, okResult.Value);
    }

    //[Fact]
    //public async Task AdicionarPessoas_ShouldReturnBadRequest_WhenModelStateIsInvalid()
    //{
    //    // Arrange
    //    _controller.ModelState.AddModelError("Name", "Required");
    //    var criarPessoasDto = new CriarPessoasDto();

    //    // Act
    //    var result = await _controller.AdicionarPessoas(criarPessoasDto);

    //    // Assert
    //    var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
    //    Assert.IsType<SerializableError>(badRequestResult.Value);
    //}

    //[Fact]
    //public async Task DeletarPessoas_ShouldReturnOk_WhenServiceReturnsSuccess()
    //{
    //    // Arrange
    //    var response = new ResponseBase<string>
    //    {
    //        Sucesso = true,
    //        Dados = "Deleted",
    //        Mensagem = "Success"
    //    };
    //    var id = Guid.NewGuid();
    //    _mockPessoasService.Setup(s => s.DeletarPessoaAsync(id)).ReturnsAsync(response);

    //    // Act
    //    var result = await _controller.DeletarPessoas(id);

    //    // Assert
    //    var okResult = Assert.IsType<OkObjectResult>(result);
    //    Assert.Equal(response, okResult.Value);
    //}

    //[Fact]
    //public async Task DeletarPessoas_ShouldReturnBadRequest_WhenIdIsEmpty()
    //{
    //    // Act
    //    var result = await _controller.DeletarPessoas(Guid.Empty);

    //    // Assert
    //    var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
    //    Assert.Equal("Id est� vazio ou � inv�lido.", badRequestResult.Value);
    //}
}
