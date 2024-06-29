using System.Security.Claims;
using API.ToDo;
using API.ToDo.DTO;
using API.ToDo.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TarefasController : ControllerBase
    {
        private readonly TarefasService service;

        public TarefasController(TarefasService _service)
        {
            service = _service;
        }

        [HttpPost]
        [Authorize]
        public async Task<RequestResponse> CriarNovaTarefa([FromBody] CriaTarefaDTO tarefa)
        {
            var IdConta = User.FindFirstValue("Id");
            return await service.CreateTask(tarefa, int.Parse(IdConta));
        }

        [HttpGet]
        [Authorize]
        public async Task<RequestResponse> ListarTarefas()
        {
            var IdConta = User.FindFirstValue("Id");
            return await service.ListTasks(int.Parse(IdConta));
        }

        [HttpPut]
        [Authorize]
        [Route("Concluir/{Id}")]
        public async Task<RequestResponse> ConcluidTarefa(int Id)
        {
            return await service.CompleteTask(Id);
        }
    }
}