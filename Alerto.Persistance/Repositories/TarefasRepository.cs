using Alerto.Common.Abstractions;
using Alerto.Common.DTO;
using Alerto.Domain.Entities;
using Alerto.Domain.Interfaces;
using Alerto.Persistance.Context;
using Microsoft.EntityFrameworkCore;

namespace Alerto.Persistance.Repositories;

public class TarefasRepository(DBToDO acessoDados, INotificacaoRepository acessoNotificacoes, ICurrentUser currentUser) : ITarefasRepository
{
    public async Task<RequestResponse> CreateTask(CriaTarefaDTO tarefa)
    {
        try
        {
            var IdLista = (await acessoDados.Listas.FirstOrDefaultAsync(l => l.Nome == tarefa.Lista)).Id;
            var IdCategoria = (await acessoDados.Categorias.FirstOrDefaultAsync(l => l.Nome == tarefa.Categoria)).Id;

            var NovaTarefa = new Tarefa
            {
                Nome = tarefa.Tarefa,
                Concluida = false,
                Descricao = tarefa.Descricao,
                CategoriaId = IdCategoria,
                DataConclusao = tarefa.Conclusao.ToString(),
                DataCriacao = DateTime.Now.ToString(),
                Prioridade = tarefa.Prioridade,
                ListaId = IdLista,
                ContaId = currentUser.ContaId
            };

            var tarefaAdded = await acessoDados.Tarefas.AddAsync(NovaTarefa);
            var task = (ListaTarefaDTO) (await GetTaskById(tarefaAdded.Entity.Id)).Target;

            var Notify = new CriarNotificacaoDTO()
            {
                Tarefa = task,
                NotificarAos = Convert.ToDateTime(tarefa.Conclusao.Subtract(DateTime.Now - tarefa.Conclusao)),
            };

            var SaveResult = await acessoDados.SaveChangesAsync();
            if (SaveResult > 0)
                await acessoNotificacoes.RegisterNotifcation(Notify);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return new RequestResponse
            {
                Mensagem = "Erro ao adicionar tarefa",
                Sucesso = false
            };
        }

        return new RequestResponse
        {
            Mensagem = "Tarefa Adicionada Com sucesso",
            Sucesso = true,
            Target = null
        };
    }

    public async Task<RequestResponse> CompleteTask(Guid tarefaId)
    {
        try
        {
            var tarefa = await acessoDados.Tarefas.AsNoTracking().FirstOrDefaultAsync(t => t.Id == tarefaId);
            tarefa.Concluida = true;
            acessoDados.Update(tarefa);
            await acessoDados.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new RequestResponse
            {
                Mensagem = e.Message,
                Sucesso = false
            };
        }

        return new RequestResponse
        {
            Mensagem = "Tarefa Concluida Com sucess!!",
            Sucesso = true
        };
    }

    public async Task<RequestResponse> ListTasks()
    {
        var listaTarefa = new List<ListaTarefaDTO>();
        try
        {
            var lista = await acessoDados.Tarefas
                .AsNoTracking()
                .Include(l => l.Categoria)
                .Include(l => l.Lista)
                .Where(l => l.ContaId == currentUser.ContaId)
                .ToListAsync();

            foreach (var tarefa in lista)
            {
                listaTarefa.Add(new ListaTarefaDTO
                {
                    Categoria = tarefa.Categoria is not null ? tarefa.Categoria.Nome : "Default",
                    Concluida = tarefa.Concluida,
                    Descricao = tarefa.Descricao,
                    Prioridade = tarefa.Prioridade,
                    Id = tarefa.Id,
                    Conclusao = DateTime.Parse(tarefa.DataConclusao),
                    Criacao = DateTime.Parse(tarefa.DataCriacao),
                    Tarefa = tarefa.Nome,
                    ListaPertencente = tarefa.Lista is not null ? tarefa.Lista.Nome : ""
                });
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return new RequestResponse
            {
                Mensagem = "Erro ao retornar lista de tarefas!",
                Sucesso = false
            };
        }

        return new RequestResponse
        {
            Mensagem = "Lista de Tarefas Retornada com sucesso!",
            Sucesso = true,
            Target = listaTarefa
        };
    }

    public async Task<RequestResponse> GetTaskById(Guid idTarefa)
    {
        try
        {
            var task = await acessoDados.Tarefas.FirstOrDefaultAsync(t => t.Id == idTarefa);
            if (task is not null)
            {
                return new RequestResponse
                {
                    Mensagem = "Tarefa encontrada!!",
                    Sucesso = false,
                    
                };
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new RequestResponse
            {
                Mensagem = $"Erro ao encontrar tarefa: {e.Message}",
                Sucesso = false
            };
        }

        return new RequestResponse
        {
            Mensagem = "Tarefa nao encontrada!!",
            Sucesso = false
        };
    }
}