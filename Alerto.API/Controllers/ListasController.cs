using System.Security.Claims;
using Alerto.Application.Services;
using Alerto.Common.Abstractions;
using Alerto.Common.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Alerto.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListasController(ListaServices listas) : ControllerBase
    {
        [HttpPost]
        [Authorize]
        public async Task<RequestResponse> NovaListaTarefas([FromBody] CriaListaTarefasDTO lista)
        {
            return await listas.NovaListaAsync(lista);
        }

        [HttpGet]
        [Authorize]
        public async Task<RequestResponse> ListarListasTarefas()
        {
            return await listas.RetornarListasAsync();
        }

        [HttpGet]
        [Route("Tarefas")]
        [Authorize]
        public async Task<RequestResponse> ListarTarefasPorLista()
        {
            return await listas.RetornarListasComTarefasAsync();
        }
    }
}