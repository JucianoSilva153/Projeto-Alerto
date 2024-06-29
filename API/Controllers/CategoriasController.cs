using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using API.ToDo;
using API.ToDo.DTO;
using API.ToDo.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly CategoriasService service;

        public CategoriasController(CategoriasService _service)
        {
            service = _service;
        }

        [HttpPost]
        public async Task<RequestResponse> CriarCategoria([FromBody] CriaCategoriaDTO novaCategoria)
        {
            var IdConta = User.FindFirstValue("Id");
            if (IdConta is not null)
                return await service.CreateCategoria(novaCategoria, int.Parse(IdConta));

            return new RequestResponse
            {
                Mensagem = "Erro ao listar Categorias",
                Sucesso = false,
                Target = null
            };
        }

        [HttpGet]
        public async Task<RequestResponse> ListarTodasCategorias()
        {
            var IdConta = User.FindFirstValue("Id");
            if (IdConta is not null)
                return await service.ListAllCategorias(int.Parse(IdConta));
            return new RequestResponse
            {
                Mensagem = "Erro ao listar Categorias",
                Sucesso = false,
                Target = null
            };
        }

        [HttpPut]
        public async Task<RequestResponse> AlterarCategoria([FromBody] ListaAlteraCategorias categoria)
        {
            return await service.EditCategoria(categoria);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<RequestResponse> ApagarCategoria(int id)
        {
            return await service.DeleteCategoria(id);
        }
    }
}