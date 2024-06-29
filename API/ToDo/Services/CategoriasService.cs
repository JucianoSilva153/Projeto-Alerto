using API.Context;
using API.ToDo.DTO;
using Microsoft.EntityFrameworkCore;

namespace API.ToDo.Services;

public class CategoriasService
{
    private DBToDO acessoDados;

    public CategoriasService(DBToDO acessoDados)
    {
        this.acessoDados = acessoDados;
    }

    public async Task<RequestResponse> CreateCategoria(CriaCategoriaDTO categoria, int Id)
    {
        var retornoCategoria = new CriaCategoriaDTO();
        try
        {
            var novaCategoria = new CategoriaModel
            {
                Nome = categoria.Categoria,
                ContaId = Id
            };

            var categoriaAdicionada = acessoDados.Categoria.Add(novaCategoria);
            await acessoDados.SaveChangesAsync();

            retornoCategoria = new CriaCategoriaDTO
            {
                Categoria = categoriaAdicionada.Entity.Nome
            };
        }
        catch (Exception e)
        {
            return new RequestResponse
            {
                Mensagem = e.Message,
                Sucesso = false
            };
        }

        return new RequestResponse
        {
            Mensagem = "Categoria Adicionada com sucesso!!",
            Sucesso = true,
            Target = retornoCategoria
        };
    }

    public async Task<RequestResponse> ListAllCategorias(int Id)
    {
        var categorias = new List<ListaAlteraCategorias>();
        try
        {
            var ListaCategorias = await acessoDados.Categoria
                .AsNoTracking()
                .Include(l => l.Tarefas)
                .Where(c => c.ContaId == Id)
                .ToListAsync();

            foreach (var categoria in ListaCategorias)
            {
                categorias.Add(new ListaAlteraCategorias()
                {
                    Id = categoria.Id,
                    NumeroTarefas = categoria.Tarefas.Count,
                    Categoria = categoria.Nome
                });
            }
            
        }
        catch (Exception e)
        {
            return new RequestResponse
            {
                Sucesso = false,
                Mensagem = e.Message
            };
        }

        return new RequestResponse
        {
            Mensagem = "Lista de Categorias Retornada com Sucesso!",
            Sucesso = true,
            Target = categorias
        };
    }

    public async Task<RequestResponse> EditCategoria(ListaAlteraCategorias categoria)
    {
        var categoriaAlterar = new CategoriaModel();
        try
        {
            categoriaAlterar = await acessoDados.Categoria.FirstAsync(e => e.Id == categoria.Id);
            categoriaAlterar.Nome = categoria.Categoria;
            acessoDados.Update(categoriaAlterar);
            await acessoDados.SaveChangesAsync();
        }
        catch (Exception e)
        {
            return new RequestResponse
            {
                Mensagem = e.Message,
                Sucesso = false
            };
        }

        return new RequestResponse
        {
            Mensagem = "Categoria alterada com sucesso!",
            Sucesso = true,
            Target = categoria
        };
    }

    public async Task<RequestResponse> DeleteCategoria(int IdCategoria)
    {
        try
        {
            var categoriaRemover = await acessoDados.Categoria
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == IdCategoria);

            if (categoriaRemover is null)
                return new RequestResponse
                {
                    Mensagem = "Categoria Nao encontrada",
                    Sucesso = false
                };
            
            acessoDados.Remove<CategoriaModel>(categoriaRemover);
            await acessoDados.SaveChangesAsync();
        }
        catch (Exception e)
        {
            return new RequestResponse
            {
                Mensagem = e.Message,
                Sucesso = false
            };
        }

        return new RequestResponse
        {
            Mensagem = "Categoria Eliminada com sucesso!!",
            Sucesso = true
        };
    }
}