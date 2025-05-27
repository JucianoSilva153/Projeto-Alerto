using Alerto.Common.Abstractions;
using Alerto.Common.DTO;
using Alerto.Domain.Interfaces;

namespace Alerto.Application.Services;

public class CategoriaService(ICategoriaRepository categoriaRepository)
{
    public async Task<RequestResponse> NovaCategoriaAsync(CriaCategoriaDTO novaCategoria)
    {
        try
        {
            return await categoriaRepository.CreateCategoria(novaCategoria);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Erro ao tentar criar categoria: {e.Message}");
        }

        return new RequestResponse
        {
            Mensagem = $"Erro ao tentar criar categoria!",
            Sucesso = false
        };
    }
    
    public async Task<RequestResponse> ListarCategoriasAsync()
    {
        try
        {
            return await categoriaRepository.ListAllCategorias();
        }
        catch (Exception e)
        {
            Console.WriteLine($"Erro ao tentar listar categorias: {e.Message}");
        }

        return new RequestResponse
        {
            Mensagem = $"Erro ao tentar listar categorias!",
            Sucesso = false
        };
    }
    
    public async Task<RequestResponse> EditarCategoriaAsync(ListaAlteraCategorias novaCategoria)
    {
        try
        {
            return await categoriaRepository.EditCategoria(novaCategoria);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Erro ao tentar editar categoria: {e.Message}");
        }

        return new RequestResponse
        {
            Mensagem = $"Erro ao tentar editar categoria!",
            Sucesso = false
        };
    }
    
    public async Task<RequestResponse> EliminarCategoriaAsync(Guid tarefaId)
    {
        try
        {
            return await categoriaRepository.DeleteCategoria(tarefaId);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Erro ao tentar eliminar categoria: {e.Message}");
        }

        return new RequestResponse
        {
            Mensagem = $"Erro ao tentar eliminar categoria!",
            Sucesso = false
        };
    }
}