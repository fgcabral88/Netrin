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
    public async Task AdicionarPessoas_DeveRetornarOk_QuandoServicoRetornarSucesso()
    {
        // Arrange

        //Cria uma resposta simulada com sucesso para o servi�o, indicando que a opera��o foi bem-sucedida e contendo um objeto ListarPessoasDto.
        var resposta = new ResponseBase<ListarPessoasDto>(true, "Sucesso", new ListarPessoasDto());

        //Cria um objeto com os dados necess�rios para adicionar uma nova pessoa. Este objeto ser� passado como par�metro para o m�todo do controlador.
        var criarPessoasDto = new CriarPessoasDto();

        //Configura o mock do servi�o (_mockPessoasService) para retornar a resposta simulada quando o m�todo AdicionarPesssoaAsync for chamado, passando o objeto criarPessoasDto.
        _mockPessoasService.Setup(s => s.AdicionarPesssoaAsync(criarPessoasDto)).ReturnsAsync(resposta);

        // Act

        //Chama o m�todo do controlador (AdicionarPessoas) que ser� testado, passando o objeto criarPessoasDto como par�metro.
        var result = await _controller.AdicionarPessoas(criarPessoasDto);

        // Assert

        //Verifica se o resultado retornado � do tipo OkObjectResult, o que indica que a opera��o foi bem-sucedida (status HTTP 200).
        var okResult = Assert.IsType<OkObjectResult>(result);

        //Verifica se o valor retornado dentro do OkObjectResult � igual � resposta simulada. Isso garante que o controlador retornou os dados esperados.
        Assert.Equal(resposta, okResult.Value);
    }

    [Fact]
    public async Task AdicionarPessoas_DeveRetornarBadRequest_QuandoModelStateForInvalido()
    {
        // Arrange
        //Adiciona um erro de valida��o no ModelState para simular que o modelo est� inv�lido. No caso, estou dizendo que o campo "Nome" � obrigat�rio.
        _controller.ModelState.AddModelError("Nome", "Obrigat�rio");

        //Cria um objeto CriarPessoasDto com dados inv�lidos (nesse caso, � apenas o objeto vazio, que causa falha na valida��o devido ao ModelState).
        var criarPessoasDto = new CriarPessoasDto();

        // Act

        //Chama o m�todo AdicionarPessoas do controlador com o objeto inv�lido (criarPessoasDto) e que possui erros de valida��o no ModelState.
        var result = await _controller.AdicionarPessoas(criarPessoasDto);

        // Assert

        //Verifica se o resultado retornado � do tipo BadRequestObjectResult, indicando que o modelo n�o passou na valida��o e retornou um erro (status HTTP 400).
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);

        //Verifica se o valor retornado dentro do BadRequestObjectResult � do tipo SerializableError, que � o tipo usado para armazenar os erros de valida��o.
        Assert.IsType<SerializableError>(badRequestResult.Value);
    }

    [Fact]
    public async Task DeletarPessoas_DeveRetornarOk_QuandoServicoRetornarSucesso()
    {

    }

    [Fact]
    public async Task DeletarPessoas_DeveRetornarBadRequest_QuandoIdEstiverVazio()
    {
        //Act
        
        //Chama o m�todo 'DeletarPessoas' passando um GUID vazio como par�metro.
        var resultado = await _controller.DeletarPessoas(Guid.Empty);

        // Assert
        
        //Verifica se o resultado retornado � do tipo 'BadRequestObjectResult'.
        var resultadoBadRequest = Assert.IsType<BadRequestObjectResult>(resultado);

        //Confirma que a mensagem de erro retornada corresponde ao esperado.
        Assert.Equal("Id est� vazio ou � inv�lido.", resultadoBadRequest.Value);
    }
}
