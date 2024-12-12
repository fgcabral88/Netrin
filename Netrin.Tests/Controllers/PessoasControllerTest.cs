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
 
        //Cria um objeto de resposta simulada com sucesso para o serviço.
        var resposta = new PaginacaoResponseBase<ListarPessoasDto>(true, "Sucesso", new List<ListarPessoasDto> { new ListarPessoasDto() }, 1);

        //Configura o mock do serviço para retornar a resposta simulada quando o método RetornarPessoaAsync for chamado.
        _mockPessoasService.Setup(s => s.RetornarPessoaAsync(1, 4)).ReturnsAsync(resposta);

        // Act

        //Chama o método do controlador que será testado (RetornarPessoas), passando os parâmetros necessários.
        var resultado = await _controller.RetornarPessoas(1, 4);

        // Assert

        //Verifica se o resultado é do tipo OkObjectResult, o que indica sucesso.
        var resultadoOk = Assert.IsType<OkObjectResult>(resultado);

        //Verifica se o valor retornado dentro do OkObjectResult é igual à resposta simulada.
        Assert.Equal(resposta, resultadoOk.Value);
    }

    [Fact]
    public async Task RetornarPessoas_DeveRetornarNotFound_QuandoServicoNaoRetornarDados()
    {
        // Arrange

        //Cria uma resposta simulada com falha, indicando que nenhum dado foi encontrado.
        var resposta = new PaginacaoResponseBase<ListarPessoasDto>(false, "Nenhum dado encontrado", null!, 1);

        //Configura o mock do serviço para retornar a resposta simulada quando o método RetornarPessoaAsync for chamado com os parâmetros 1 e 4.
        _mockPessoasService.Setup(s => s.RetornarPessoaAsync(1, 4)).ReturnsAsync(resposta);

        // Act

        //Chama o método do controlador (RetornarPessoas) que será testado, passando os parâmetros 1 e 4.
        var resultado = await _controller.RetornarPessoas(1, 4);

        // Assert

        //Verifica se o resultado retornado é do tipo NotFoundObjectResult, o que indica que nenhum dado foi encontrado.
        var restauldoNotFound = Assert.IsType<NotFoundObjectResult>(resultado);

        //Verifica se a mensagem retornada dentro do NotFoundObjectResult corresponde à mensagem da resposta simulada.
        Assert.Equal(resposta.Mensagem, restauldoNotFound.Value);
    }

    [Fact]
    public async Task RetornarPessoasId_DeveRetornarOk_QuandoServicoRetornarSucesso()
    {
        // Arrange

        //Cria uma resposta simulada com sucesso para o serviço, com um objeto ListarPessoasDto e status 'true' indicando sucesso.
        var resposta = new ResponseBase<ListarPessoasDto>(true, "Sucesso", new ListarPessoasDto());
        //Cria um Id único usando Guid.NewGuid() para simular a busca de uma pessoa específica.
        var id = Guid.NewGuid();

        //Configura o mock do serviço (_mockPessoasService) para retornar a resposta simulada quando o método RetornarPessoaIdAsync for chamado com o Id gerado.
        _mockPessoasService.Setup(s => s.RetornarPessoaIdAsync(id)).ReturnsAsync(resposta);

        // Act

        //Chama o método do controlador (RetornarPessoasId) que será testado, passando o ID gerado como parâmetro.
        var resultado = await _controller.RetornarPessoasId(id);

        // Assert

        //Verifica se o resultado retornado é do tipo OkObjectResult, indicando sucesso e que o status HTTP retornado é 200 (OK).
        var resultadoOk = Assert.IsType<OkObjectResult>(resultado);

        //Verifica se o valor retornado dentro do OkObjectResult é igual à resposta simulada, garantindo que os dados retornados estão corretos.
        Assert.Equal(resposta, resultadoOk.Value);
    }

    [Fact]
    public async Task RetornarPessoasId_DeveRetornarBadRequest_QuandoIdEstiverVazio()
    {
        // Act

        //Chama o método do controlador (RetornarPessoasId) que será testado, passando um ID vazio (Guid.Empty).
        var resultado = await _controller.RetornarPessoasId(Guid.Empty);

        // Assert

        //Verifica se o resultado retornado é do tipo BadRequestObjectResult, indicando que o ID vazio causou uma resposta de erro (400).
        var resultadoBadRequest = Assert.IsType<BadRequestObjectResult>(resultado);

        //Verifica se a mensagem de erro retornada dentro do BadRequestObjectResult corresponde à mensagem esperada ("Id informado é inválido.").
        Assert.Equal("Id informado é inválido.", resultadoBadRequest.Value);
    }

    [Fact]
    public async Task AdicionarPessoas_DeveRetornarOk_QuandoServicoRetornarSucesso()
    {
        // Arrange

        //Cria uma resposta simulada com sucesso para o serviço, indicando que a operação foi bem-sucedida e contendo um objeto ListarPessoasDto.
        var resposta = new ResponseBase<ListarPessoasDto>(true, "Sucesso", new ListarPessoasDto());

        //Cria um objeto com os dados necessários para adicionar uma nova pessoa. Este objeto será passado como parâmetro para o método do controlador.
        var criarPessoasDto = new CriarPessoasDto();

        //Configura o mock do serviço (_mockPessoasService) para retornar a resposta simulada quando o método AdicionarPesssoaAsync for chamado, passando o objeto criarPessoasDto.
        _mockPessoasService.Setup(s => s.AdicionarPesssoaAsync(criarPessoasDto)).ReturnsAsync(resposta);

        // Act

        //Chama o método do controlador (AdicionarPessoas) que será testado, passando o objeto criarPessoasDto como parâmetro.
        var result = await _controller.AdicionarPessoas(criarPessoasDto);

        // Assert

        //Verifica se o resultado retornado é do tipo OkObjectResult, o que indica que a operação foi bem-sucedida (status HTTP 200).
        var okResult = Assert.IsType<OkObjectResult>(result);

        //Verifica se o valor retornado dentro do OkObjectResult é igual à resposta simulada. Isso garante que o controlador retornou os dados esperados.
        Assert.Equal(resposta, okResult.Value);
    }

    [Fact]
    public async Task AdicionarPessoas_DeveRetornarBadRequest_QuandoModelStateForInvalido()
    {
        // Arrange
        //Adiciona um erro de validação no ModelState para simular que o modelo está inválido. No caso, estou dizendo que o campo "Nome" é obrigatório.
        _controller.ModelState.AddModelError("Nome", "Obrigatório");

        //Cria um objeto CriarPessoasDto com dados inválidos (nesse caso, é apenas o objeto vazio, que causa falha na validação devido ao ModelState).
        var criarPessoasDto = new CriarPessoasDto();

        // Act

        //Chama o método AdicionarPessoas do controlador com o objeto inválido (criarPessoasDto) e que possui erros de validação no ModelState.
        var result = await _controller.AdicionarPessoas(criarPessoasDto);

        // Assert

        //Verifica se o resultado retornado é do tipo BadRequestObjectResult, indicando que o modelo não passou na validação e retornou um erro (status HTTP 400).
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);

        //Verifica se o valor retornado dentro do BadRequestObjectResult é do tipo SerializableError, que é o tipo usado para armazenar os erros de validação.
        Assert.IsType<SerializableError>(badRequestResult.Value);
    }

    [Fact]
    public async Task EditarPessoas_Sucesso_DeveRetornarOk()
    {
        // Arrange

        //Criação de um objeto de entrada com dados válidos
        var editarPessoaDto = new EditarPessoasDto()
        {
            Id = Guid.NewGuid(),
            Nome = "Nome teste",
            Sobrenome = "Sobrenome Teste",
            DataNascimento = DateTime.Now,
            Email = "email.teste@example.com",
            Telefone = "41999999999",
            Cidade = "Curitiba",
            Estado = "PR"
        };

        //Criação de uma instância de resposta simulada do serviço
        var listarPessoasDto = new ListarPessoasDto
        {
            Id = Guid.NewGuid(),
            Nome = "Nome teste",
            Sobrenome = "Sobrenome Teste",
            DataNascimento = DateTime.Now,
            Email = "email.teste@example.com",
            Telefone = "41999999999",
            Cidade = "Curitiba",
            Estado = "PR"
        };

        //Configuração do mock para retornar a resposta esperada
        var pessoaResposta = new ResponseBase<ListarPessoasDto>(true, "Pessoa editada com sucesso", listarPessoasDto);

        _mockPessoasService.Setup(service => service.EditarPessoaAsync(editarPessoaDto)).ReturnsAsync(pessoaResposta);

        // Act

        //Executa a ação da controller com os dados de entrada
        var resultado = await _controller.EditarPessoas(editarPessoaDto);

        // Assert

        //Verifica se o resultado é do tipo esperado e contém os dados corretos
        var resultadoOk = Assert.IsType<OkObjectResult>(resultado);

        //Verifica se o valor não é nulo
        Assert.NotNull(resultadoOk.Value);

        //Confirma que o valor retornado é do tipo ResponseBase com os dados esperados
        var resposta = Assert.IsType<ResponseBase<ListarPessoasDto>>(resultadoOk.Value);

        //Verifica se os dados retornados não são nulos
        Assert.NotNull(resposta.Dados);
        
        //Confirma o tipo do objeto nos dados retornados
        Assert.IsType<ListarPessoasDto>(resposta.Dados);
    }

    [Fact]
    public async Task EditarPessoas_NaoEncontrada_DeveRetornarNotFound()
    {
        // Arrange

        //Criação de um objeto de entrada com dados válidos
        var editarPessoaDto = new EditarPessoasDto()
        {
            Id = Guid.NewGuid(),
            Nome = "Nome teste",
            Sobrenome = "Sobrenome Teste",
            DataNascimento = DateTime.Now,
            Email = "email.teste@example.com",
            Telefone = "41999999999",
            Cidade = "Curitiba",
            Estado = "PR"
        };

        var pessoaResposta = new ResponseBase<ListarPessoasDto>(false, "Pessoa não encontrada", null);

        _mockPessoasService.Setup(service => service.EditarPessoaAsync(editarPessoaDto)).ReturnsAsync(pessoaResposta);

        // Act
        var result = await _controller.EditarPessoas(editarPessoaDto);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("Pessoa não encontrada", notFoundResult.Value);
    }

    [Fact]
    public async Task DeletarPessoas_DeveRetornarOk_QuandoServicoRetornarSucesso()
    {
        //Arrange

        //Cria um Id de pessoa válido para simular a exclusão.
        var pessoaId = Guid.NewGuid();

        //Cria uma resposta simulada do serviço indicando sucesso na operação
        var pessoaResposta = new ResponseBase<ListarPessoasDto>(true, "Pessoa deletada com sucesso", null);

        //Configura o mock do serviço para retornar a resposta simulada quando chamado com o ID fornecido
        _mockPessoasService.Setup(service => service.DeletarPessoaAsync(pessoaId)).ReturnsAsync(pessoaResposta);

        //Act

        //Executa o método da controller que está sendo testado
        var resultado = await _controller.DeletarPessoas(pessoaId);

        // Assert

        //Garante que o resultado é do tipo OkObjectResult
        var resultadoOk = Assert.IsType<OkObjectResult>(resultado);

        //Verifica que o valor do resultado não é nulo
        Assert.NotNull(resultadoOk.Value);

        //Confirma que o valor retornado é do tipo esperado (ResponseBase com ListarPessoasDto)
        var response = Assert.IsType<ResponseBase<ListarPessoasDto>>(resultadoOk.Value);

        //Verifica que a operação foi bem-sucedida
        Assert.True(response.Sucesso);

        //Confirma que a mensagem de sucesso corresponde à esperada
        Assert.Equal("Pessoa deletada com sucesso", response.Mensagem);
    }

    [Fact]
    public async Task DeletarPessoas_DeveRetornarBadRequest_QuandoIdEstiverVazio()
    {
        //Act
        
        //Chama o método 'DeletarPessoas' passando um GUID vazio como parâmetro.
        var resultado = await _controller.DeletarPessoas(Guid.Empty);

        //Assert
        
        //Verifica se o resultado retornado é do tipo 'BadRequestObjectResult'.
        var resultadoBadRequest = Assert.IsType<BadRequestObjectResult>(resultado);

        //Confirma que a mensagem de erro retornada corresponde ao esperado.
        Assert.Equal("Id está vazio ou é inválido.", resultadoBadRequest.Value);
    }
}
