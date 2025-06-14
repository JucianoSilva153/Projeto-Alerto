using System.Security.Claims;
using Alerto.API.ToDo.Services;
using Alerto.API.ToDo.Services.OmbalaService.Models;
using Alerto.Application.Services;
using Alerto.Common.Abstractions;
using Alerto.Common.DTO;
using Alerto.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Alerto.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContaController(ContaService contas, AuthService authService) : ControllerBase
    {
        [HttpPost]
        [AllowAnonymous]
        [Route("autenticar")]
        public async Task<IActionResult> GerarToken([FromBody] AutenticarContaDTO conta)
        {
            var token = await authService.GerarToken(conta);

            if (token == String.Empty)
                return BadRequest(new RequestResponse()
                {
                    Mensagem = "Erro ao Autenticar",
                    Sucesso = false
                });

            return Ok(new RequestResponse
            {
                Mensagem = "Autenticacao Feita com Sucesso!",
                Target = token,
                Sucesso = true
            });
        }

        [HttpGet("message")]
        [AllowAnonymous]
        public async Task<IActionResult> SendMessage([FromServices] ISMSSender sender)
        {
            try
            {
                var result = await sender.SendMessage(new Message
                {
                    from = "923679528",
                    message = "Testando...",
                    to = "921765383"
                });

                if (result)
                    return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return Problem();
        }

        [HttpGet("login-google")]
        [AllowAnonymous]
        public IActionResult LoginComGoole()
        {
            var redirectUrl = Url.Action("AutenticarViaGoogle", "Conta");
            var props = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(props, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet("autenticar-google")]
        [AllowAnonymous]
        public async Task<IActionResult> AutenticarViaGoogle([FromQuery] string returnUrl = null)
        {
            var result = await HttpContext.AuthenticateAsync();

            if (!result.Succeeded)
                return Unauthorized(new RequestResponse
                {
                    Mensagem = "Erro ao autenticar com Google!",
                    Sucesso = false
                });

            var claims = result.Principal?.Identities?.FirstOrDefault()?.Claims;
            var email = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var nome = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            var googleId = claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            var conta = await contas.RetornarContaAtualAsync();
            if (!conta.Sucesso)
            {
                await contas.NovaContaAsync(new AdicionarEditarContaDTO
                {
                    Email = email,
                    Nome = nome,
                    Password = "000000",
                    GoogleId = googleId
                });
            }

            var token = await authService.GerarTokenViaGoogle(email, googleId);

            if (string.IsNullOrEmpty(token))
                return Unauthorized(new RequestResponse
                {
                    Mensagem = "Não foi possível autenticar!",
                    Sucesso = false
                });

            // 🔄 Se returnUrl veio na query, redireciona para lá com o token
            if (!string.IsNullOrEmpty(returnUrl))
                return Redirect($"{returnUrl}?token={token}");


            // Fallback: redireciona para rota padrão
            return Redirect($"http://localhost:5018/autenticado?token={token}");
        }


        [HttpGet]
        [Route("Autenticado")]
        [Authorize]
        public IActionResult VerificarAuntenticacao()
        {
            return Ok("Autenticado");
        }

        [HttpGet]
        [Route("Resumo")]
        [Authorize]
        public async Task<RequestResponse> RetornarResumo()
        {
            return await contas.RetornarResumoContaAsync();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<RequestResponse> CriarConta([FromBody] AdicionarEditarContaDTO conta)
        {
            return await contas.NovaContaAsync(conta);
        }
    }
}