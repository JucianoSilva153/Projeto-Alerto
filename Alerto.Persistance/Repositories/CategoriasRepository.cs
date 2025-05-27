using Alerto.Common.Abstractions;
using Alerto.Common.DTO;
using Alerto.Domain.Entities;
using Alerto.Domain.Interfaces;
using Alerto.Persistance.Context;
using Microsoft.EntityFrameworkCore;

namespace Alerto.Persistance.Repositories;

public class CategoriasRepository : ICategoriaRepository
{
    private readonly DBToDO _acessoDados;
    private readonly ICurrentUser _currentUser;

    public CategoriasRepository(DBToDO acessoDados, ICurrentUser currentUser)
    {
        _acessoDados = acessoDados;
        _currentUser = currentUser;
    }

    public async Task<RequestResponse> CreateCategoria(CriaCategoriaDTO categoria)
    {
        var retornoCategoria = new CriaCategoriaDTO();
        try
        {
            var novaCategoria = new Categoria
            {
                Nome = categoria.Categoria,
                ContaId = _currentUser.ContaId
            };

            var categoriaAdicionada = _acessoDados.Categorias.Add(novaCategoria);
            await _acessoDados.SaveChangesAsync();

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

    public async Task<RequestResponse> ListAllCategorias()
    {
        var categorias = new List<ListaAlteraCategorias>();
        try
        {
            var ListaCategorias = await _acessoDados.Categorias
                .AsNoTracking()
                .Include(l => l.Tarefas)
                .Where(c => c.ContaId == _currentUser.ContaId)
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
        var categoriaAlterar = new Categoria();
        try
        {
            categoriaAlterar = await _acessoDados.Categorias.FirstAsync(e => e.Id == categoria.Id && e.ContaId == _currentUser.ContaId);
            categoriaAlterar.Nome = categoria.Categoria;
            _acessoDados.Update(categoriaAlterar);
            await _acessoDados.SaveChangesAsync();
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

    public async Task<RequestResponse> DeleteCategoria(Guid IdCategoria)
    {
        try
        {
            var categoriaRemover = await _acessoDados.Categorias
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == IdCategoria);

            if (categoriaRemover is null)
                return new RequestResponse
                {
                    Mensagem = "Categoria Nao encontrada",
                    Sucesso = false
                };

            _acessoDados.Remove<Categoria>(categoriaRemover);
            await _acessoDados.SaveChangesAsync();
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