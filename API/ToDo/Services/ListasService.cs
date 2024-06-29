using API.Context;
using API.ToDo.DTO;
using Microsoft.EntityFrameworkCore;

namespace API.ToDo.Services;

public class ListasService
{
    private DBToDO acessoDados;

    public ListasService(DBToDO acessoDados)
    {
        this.acessoDados = acessoDados;
    }

    public async Task<RequestResponse> CreateTaskList(CriaListaTarefasDTO lista)
    {
        try
        {
            acessoDados.Lista.Add(new ListaModel()
            {
                Nome = lista.Lista,
                ContaId = lista.ContaID
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

    public async Task<RequestResponse> ListTaskList(int Id)
    {
        var listasTarefa = new List<ListaAlteraListaTarefaDTO>();
        try
        {
            var listas = await acessoDados.Lista
                .AsNoTracking()
                .Include(l => l.Tarefas)
                .Where(l => l.ContaId == Id)
                .ToListAsync();

            foreach (var lista in listas)
            {
                listasTarefa.Add(new ListaAlteraListaTarefaDTO()
                {
                    Id = lista.Id,
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
            var tarefasPorLista = await acessoDados.Lista
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
                                    Conclusao = ConverterData(tarefa.DataConclusao),
                                    Criacao = ConverterData(tarefa.DataCriacao),
                                    Prioridade = tarefa.Prioridade,
                                    Descricao = tarefa.Descricao,
                                    Tarefa = tarefa.Nome,
                                    ListaPertencente = tarefa.Lista.Nome,
                                    Id = tarefa.Id
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

    private DateTime ConverterData(string data)
    {
        var dataSplited = data.Split("/");
        return new DateTime(int.Parse(dataSplited[2]), int.Parse(dataSplited[2]), int.Parse(dataSplited[2]));
    }
}