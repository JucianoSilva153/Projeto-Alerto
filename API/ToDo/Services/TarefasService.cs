using API.Context;
using API.ToDo.DTO;
using Microsoft.EntityFrameworkCore;

namespace API.ToDo.Services;

public class TarefasService
{
    private DBToDO acessoDados;
    private NotificacaoService NotificacaoService;

    public TarefasService(DBToDO acessoDados)
    {
        this.acessoDados = acessoDados;
        NotificacaoService = new NotificacaoService(acessoDados);
    }

    public async Task<RequestResponse> CreateTask(CriaTarefaDTO tarefa, int IdConta)
    {
        try
        {
            var IdLista = (await acessoDados.Lista.FirstOrDefaultAsync(l => l.Nome == tarefa.Lista)).Id;
            var IdCategoria = (await acessoDados.Categoria.FirstOrDefaultAsync(l => l.Nome == tarefa.Categoria)).Id;

            var NovaTarefa = new TarefaModel
            {
                Nome = tarefa.Tarefa,
                Concluida = false,
                Descricao = tarefa.Descricao,
                CategoriaId = IdCategoria,
                DataConclusao = tarefa.Conclusao.ToString(),
                DataCriacao = DateTime.Now.ToString(),
                Prioridade = tarefa.Prioridade,
                ListaId = IdLista,
                ContaId = IdConta
            };
            
            var tarefaAdded = await acessoDados.Tarefa.AddAsync(NovaTarefa);
            var Notify = new CriarNotificacaoDTO()
            {
                Tarefa = tarefaAdded.Entity,
                NotificarAos = Convert.ToDateTime(tarefa.Conclusao.Subtract(DateTime.Now - tarefa.Conclusao)),
            };

            var SaveResult = await acessoDados.SaveChangesAsync();
            if (SaveResult > 0)
                await NotificacaoService.RegisterNotifcation(Notify, IdConta);

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

    public async Task<RequestResponse> CompleteTask(int Id)
    {
        try
        {
            var tarefa = await acessoDados.Tarefa.AsNoTracking().FirstOrDefaultAsync(t => t.Id == Id);
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

    public async Task<RequestResponse> ListTasks(int IdConta)
    {
        var listaTarefa = new List<ListaTarefaDTO>();
        try
        {
            var lista = await acessoDados.Tarefa
                .AsNoTracking()
                .Include(l => l.Categoria)
                .Include(l => l.Lista)
                .Where(l => l.ContaId == IdConta)
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
}