using Alerto.Common.Abstractions;
using Alerto.Common.DTO;
using Alerto.Common.Extensions;
using Alerto.Domain.Entities;
using Alerto.Domain.Interfaces;
using Alerto.Persistance.Context;
using Microsoft.EntityFrameworkCore;

namespace Alerto.Persistance.Repositories;

public class ListasRepository(DBToDO acessoDados, ICurrentUser currentUser) : IListasRepository
{
    public async Task<RequestResponse> CreateTaskList(CriaListaTarefasDTO lista)
    {
        try
        {
            acessoDados.Listas.Add(new Lista()
            {
                Nome = lista.Lista,
                ContaId = currentUser.ContaId
            });

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
            Mensagem = "Lista de tarefas adicionada com sucesso!",
            Sucesso = true,
            Target = lista
        };
    }
    public async Task<RequestResponse> ListTaskList()
    {
        var listasTarefa = new List<ListaAlteraListaTarefaDTO>();
        try
        {
            var listas = await acessoDados.Listas
                .AsNoTracking()
                .Include(l => l.Tarefas)
                .Where(l => l.ContaId == currentUser.ContaId)
                .ToListAsync();

            foreach (var lista in listas)
            {
                listasTarefa.Add(new ListaAlteraListaTarefaDTO()
                {
                    Id = currentUser.ContaId,
                    NumeroTarefas = lista.Tarefas.Count,
                    Lista = lista.Nome
                });
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return new RequestResponse
        {
            Mensagem = "Listas de tarefas listadas com sucesso",
            Sucesso = true,
            Target = listasTarefa
        };
    }
    public async Task<RequestResponse> ListTasksPerList()
    {
        var listasTarefas = new List<TarefasDeUmaListaDTO>();
        try
        {
            var tarefasPorLista = await acessoDados.Listas
                .AsNoTracking()
                .Include(l => l.Tarefas)!
                .ThenInclude(c => c.Categoria)
                .Include(l => l.Tarefas)
                .ToListAsync();


            foreach (var listas in tarefasPorLista)
            {
                if (listas.Tarefas is not null)
                    foreach (var tarefa in listas.Tarefas)
                    {
                        listasTarefas.Add(new TarefasDeUmaListaDTO
                        {
                            Lista = listas.Nome,
                            Tarefas = new List<ListaTarefaDTO>
                            {
                                //TODO: Criar uma funcao para converter a data em string para DateTime, ou entao, tranformar o tipo de dados na model de string para dateTime
                                new ListaTarefaDTO()
                                {
                                    Categoria = tarefa.Categoria?.Nome,
                                    Concluida = tarefa.Concluida,
                                    Conclusao = tarefa.DataConclusao.ToDateTime(),
                                    Criacao = tarefa.DataCriacao.ToDateTime(),
                                    Prioridade = tarefa.Prioridade,
                                    Descricao = tarefa.Descricao,
                                    Tarefa = tarefa.Nome,
                                    ListaPertencente = tarefa.Lista.Nome,
                                    Id = currentUser.ContaId
                                }
                            }
                        });
                    }
            }
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
            Mensagem = "Listas e Suas Tarefas retornada com sucesso",
            Sucesso = true,
            Target = listasTarefas
        };
    }
}