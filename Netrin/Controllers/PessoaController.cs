using Microsoft.AspNetCore.Mvc;
using Netrin.Application.Dtos.Pessoa;
using Netrin.Domain.Service.Interfaces.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace Netrin.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PessoaController : ControllerBase
    {
        private readonly IPessoaService _pessoaService;
        public PessoaController(IPessoaService pessoaService)
        {
            _pessoaService = pessoaService;
        }

        [HttpGet]
        [Route("RetornarPessoaAsync")]
        [SwaggerOperation(Summary = "Retornar todas as Pessoas.", Description = "Retorna uma lista com todas as Pessoas disponíveis no sistema.")]
        [SwaggerResponse(200, "Lista de Pessoas retornada com sucesso.", typeof(IEnumerable<ListarPessoaDto>))]
        [SwaggerResponse(404, "Nenhuma Pessoa encontrada.")]
        [SwaggerResponse(500, "Erro interno ao processar a solicitação.")]
        public async Task<IActionResult> RetornarPessoaAsync()
        {
            var pessoasResponse = await _pessoaService.RetornarPessoaAsync();

            if(!pessoasResponse.Sucesso)
            {
                if(pessoasResponse.Dados is null)
                    return NotFound(pessoasResponse.Mensagem);

                return BadRequest(pessoasResponse.Mensagem);
            }

            return Ok(pessoasResponse.Dados);
        }
    }
}
