using Moq;
using Netrin.Api.Presentation.Controllers;
using Netrin.Domain.Service.Interfaces.Services;

public class PessoasControllerTests
{
    private readonly Mock<IPessoasService> _mockPessoaService;
    private readonly PessoasController _controller;

    public PessoasControllerTests()
    {
        _mockPessoaService = new Mock<IPessoasService>();
        _controller = new PessoasController(_mockPessoaService.Object);
    }
}