using System.Security.Claims;
using API.ToDo;
using API.ToDo.DTO;
using API.ToDo.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListasController : ControllerBase
    {
        private readonly ListasService service;

        public ListasController(ListasService _service)
        {
            service = _service;
        }

        [HttpPost]
        [Authorize]
        public async Task<RequestResponse> NovaListaTarefas([FromBody] CriaListaTarefasDTO lista)
        {
            return await service.CreateTaskList(lista);
        }

        [HttpGet]
        [Authorize]
        public async Task<RequestResponse> ListarListasTarefas()
        {
            if (User.FindFirstValue("Id") != null)
            {
                var IdConta = int.Parse(User.FindFirstValue("Id"));
                return await service.ListTaskList(IdConta);
            }

            return new RequestResponse()
            {
                Mensagem = "Erro ao Retornar Listas",
                Target = null,
                Sucesso = false
            };
        }

        [HttpGet]
        [Route("Tarefas")]
        [Authorize]
        public async Task<RequestResponse> ListarTarefasPorLista()
        {
            return await service.ListTasksPerList();
        }
    }
}