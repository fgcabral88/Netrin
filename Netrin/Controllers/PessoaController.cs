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

        /// <summary>
        /// Retorna todas as Pessoas cadastradas.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("RetornarPessoas")]
        [SwaggerOperation(Summary = "Retornar todas as Pessoas.", Description = "Retorna uma lista com todas as Pessoas disponíveis no sistema.")]
        [SwaggerResponse(200, "Lista de Pessoas retornada com sucesso.", typeof(IEnumerable<ListarPessoaDto>))]
        [SwaggerResponse(404, "Nenhuma Pessoa encontrada.")]
        [SwaggerResponse(500, "Erro interno ao processar a solicitação.")]
        public async Task<IActionResult> RetornarPessoas()
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

        /// <summary>
        /// Retorna uma Pessoa pelo Id informado.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("RetornarPessoaPorId")]
        [SwaggerOperation(Summary = "Retornar uma Pessoa pelo Id.", Description = "Retorna uma Pessoa pelo Id informado.")]
        [SwaggerResponse(200, "Pessoa retornada com sucesso.", typeof(ListarPessoaDto))]
        [SwaggerResponse(404, "Nenhuma Pessoa encontrada.")]
        [SwaggerResponse(500, "Erro interno ao processar a solicitação.")]
        public async Task<IActionResult> RetornarPessoaPorId(Guid id)
        {
            if(id == Guid.Empty)
                return BadRequest("Id informado é inválido.");

            var pessoaIdresponse = await _pessoaService.RetornarPessoaIdAsync(id);

            if (!pessoaIdresponse.Sucesso)
            {
                if (pessoaIdresponse.Dados is null)
                    return NotFound(pessoaIdresponse.Mensagem);

                return BadRequest(pessoaIdresponse.Mensagem);
            }

            return Ok(pessoaIdresponse.Dados);
        }
    }
}
