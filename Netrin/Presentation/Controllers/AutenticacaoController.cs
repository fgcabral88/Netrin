using Microsoft.AspNetCore.Mvc;
using Netrin.Application.Dtos.Login;
using Netrin.Application.Helpers;
using Swashbuckle.AspNetCore.Annotations;

namespace Netrin.Api.Presentation.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class AutenticacaoController : ControllerBase
    {
        private readonly JwtTokenHelper _jwtTokenHelper;

        public AutenticacaoController(JwtTokenHelper jwtTokenHelper)
        {
            _jwtTokenHelper = jwtTokenHelper;
        }

        /// <summary>
        /// Realiza o login do usuário.
        /// </summary>
        /// <param name="loginDto"></param>
        /// <returns></returns>
        [SwaggerOperation (Summary = "Realiza o login do usuário", Description = "Realiza o login do usuário no sistema")]
        [SwaggerResponse(200, "Usuário logado com sucesso", typeof(LoginDto))]
        [SwaggerResponse(401, "Usuário ou senha inválidos")]
        [HttpPost("login")]
        public IActionResult Login([FromForm] LoginDto loginDto)
        {
            // Substitua por sua lógica de autenticação.
            if (loginDto.Login == "Felipe" && loginDto.Senha == "Netrin")
            {
                var token = _jwtTokenHelper.GenerateToken("1", "Admin");
                return Ok(new { Token = token });
            }

            return Unauthorized("Credenciais inválidas");
        }
    }
}
