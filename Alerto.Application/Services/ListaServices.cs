using System;
using System.Threading.Tasks;
using Alerto.Common.Abstractions;
using Alerto.Common.DTO;
using Alerto.Domain.Interfaces;

namespace Alerto.Application.Services;

public class ListaServices(IListasRepository listasRepository)
{
    public async Task<RequestResponse> NovaListaAsync(CriaListaTarefasDTO novalista)
    {
        try
        {
            return await listasRepository.CreateTaskList(novalista);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Erro ao tentar criar lista de tarefas: {e.Message}");
        }

        return new RequestResponse
        {
            Mensagem = "Erro ao tentar criar lista de tarefas!!",
            Sucesso = false
        };
    }
    
    public async Task<RequestResponse> RetornarListasAsync()
    {
        try
        {
            return await listasRepository.ListTaskList();
        }
        catch (Exception e)
        {
            Console.WriteLine($"Erro ao tentar retornar lista de tarefas: {e.Message}");
        }

        return new RequestResponse
        {
            Mensagem = "Erro ao tentar retornar lista de tarefas!!",
            Sucesso = false
        };
    }
    
    public async Task<RequestResponse> RetornarListasComTarefasAsync()
    {
        try
        {
            return await listasRepository.ListTasksPerList();
        }
        catch (Exception e)
        {
            Console.WriteLine($"Erro ao tentar retornar lista de tarefas: {e.Message}");
        }

        return new RequestResponse
        {
            Mensagem = "Erro ao tentar retornar lista de tarefas!!",
            Sucesso = false
        };
    }
}