using System.ComponentModel.DataAnnotations;
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
    public class CategoriasController(CategoriaService categorias) : ControllerBase
    {
        [HttpPost]
        [Authorize]
        public async Task<RequestResponse> CriarCategoria([FromBody] CriaCategoriaDTO novaCategoria)
        {
            return await categorias.NovaCategoriaAsync(novaCategoria);
        }

        [HttpGet]
        [Authorize]
        public async Task<RequestResponse> ListarTodasCategorias()
        {
            return await categorias.ListarCategoriasAsync();
        }

        [HttpPut]
        [Authorize]
        public async Task<RequestResponse> AlterarCategoria([FromBody] ListaAlteraCategorias categoria)
        {
            return await categorias.EditarCategoriaAsync(categoria);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        [Authorize]
        public async Task<RequestResponse> ApagarCategoria(Guid id)
        {
            return await categorias.EliminarCategoriaAsync(id);
        }
    }
}