using System;
using System.Threading.Tasks;
using Alerto.Common.Abstractions;
using Alerto.Common.DTO;
using Alerto.Domain.Interfaces;
using Alerto.Infrastructure.Services.ModoChatoService;

namespace Alerto.Application.Services;

public class TarefaService(ITarefasRepository tarefasRepository, IAlertoService alertoService)
{
    public async Task<RequestResponse> NovaTarefaAsync(CriaTarefaDTO novaTarefa)
    {
        try
        {
            var result = await tarefasRepository.CreateTask(novaTarefa);
            if (result.Sucesso && novaTarefa.ModoChato is not null)
                await alertoService.AgendarAlarme(novaTarefa.ModoChato);

            return result;
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
            var result = await tarefasRepository.CompleteTask(tarefaId);
            if (result.Sucesso)
                await alertoService.CancelarAlarmes(tarefaId);

            return result;
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