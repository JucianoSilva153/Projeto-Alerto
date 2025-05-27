using Alerto.Common.Abstractions;
using Alerto.Common.DTO;
using Alerto.Domain.Interfaces;

namespace Alerto.Application.Services;

public class TarefaService(ITarefasRepository tarefasRepository)
{
    public async Task<RequestResponse> NovaTarefaAsync(CriaTarefaDTO novaTarefa)
    {
        try
        {
            return await tarefasRepository.CreateTask(novaTarefa);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Erro ao tentar criar Tarefa: {e.Message}");
        }

        return new RequestResponse
        {
            Mensagem = "Erro ao tentar criar Tarefa!!",
            Sucesso = false
        };
    }
    
    public async Task<RequestResponse> ConcluirTarefaAsync(Guid tarefaId)
    {
        try
        {
            return await tarefasRepository.CompleteTask(tarefaId);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Erro ao tentar concluir Tarefa: {e.Message}");
        }

        return new RequestResponse
        {
            Mensagem = "Erro ao tentar concluir Tarefa!!",
            Sucesso = false
        };
    }
    
    public async Task<RequestResponse> RetornarTarefasAsync()
    {
        try
        {
            return await tarefasRepository.ListTasks();
        }
        catch (Exception e)
        {
            Console.WriteLine($"Erro ao tentar retornar tarefas: {e.Message}");
        }

        return new RequestResponse
        {
            Mensagem = "Erro ao tentar retornar tarefas!!",
            Sucesso = false
        };
    }
    
    public async Task<RequestResponse> RetornarTarefaPeloIdAsync(Guid tarefaId)
    {
        try
        {
            return await tarefasRepository.GetTaskById(tarefaId);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Erro ao tentar retornar tarefa: {e.Message}");
        }

        return new RequestResponse
        {
            Mensagem = "Erro ao tentar retornar tarefa!!",
            Sucesso = false
        };
    }
}