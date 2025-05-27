using Alerto.Application.Services;
using Alerto.Common.Abstractions;
using Alerto.Common.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Alerto.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TarefasController(TarefaService tarefas) : ControllerBase
    {
        [HttpPost]
        [Authorize]
        public async Task<RequestResponse> CriarNovaTarefa([FromBody] CriaTarefaDTO tarefa)
        {
            return await tarefas.NovaTarefaAsync(tarefa);
        }

        [HttpGet]
        [Authorize]
        public async Task<RequestResponse> ListarTarefas()
        {
            return await tarefas.RetornarTarefasAsync();
        }

        [HttpPut]
        [Authorize]
        [Route("Concluir/{Id:guid}")]
        public async Task<RequestResponse> ConcluidTarefa(Guid Id)
        {
            return await tarefas.ConcluirTarefaAsync(Id);
        }
    }
}