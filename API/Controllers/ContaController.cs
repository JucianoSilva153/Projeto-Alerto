using API.ToDo;
using API.ToDo.DTO;
using API.ToDo.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContaController : ControllerBase
    {
        private readonly ContasService service;
        private readonly AuthService authService;

        public ContaController(ContasService service, AuthService authService)
        {
            this.service = service;
            this.authService = authService;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("autenticar")]
        public async Task<RequestResponse> GerarToken([FromBody] AutenticarContaDTO conta)
        {
            var token = await authService.GerarToken(conta);

            if (token == String.Empty)
                return new RequestResponse()
                {
                    Mensagem = "Erro ao Autenticar",
                    Sucesso = false
                };

            return new RequestResponse
            {
                Mensagem = "Autenticacao Feita com Sucesso!",
                Target = token,
                Sucesso = true
            };
        }

        [HttpGet]
        [Route("Autenticado")]
        [Authorize]
        public IActionResult VerificarAuntenticacao()
        {
            return Ok("Autenticado");
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<RequestResponse> CriarConta([FromBody] AdicionarEditarContaDTO conta)
        {
            return await service.CreateAccount(conta);
        }

        [HttpGet]
        [Authorize]
        [Route("PorEmail/{email}")]
        public async Task<RequestResponse> RetornarContaPeloEmail(string email)
        {
            return await service.GetAccountByEmail(email);
        }
    }
}