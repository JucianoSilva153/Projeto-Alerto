using Alerto.Common.Abstractions;
using Alerto.Common.DTO;

namespace Alerto.Domain.Interfaces;

public interface ITarefasRepository
{
    public Task<RequestResponse> CreateTask(CriaTarefaDTO tarefa);
    public Task<RequestResponse> CompleteTask(Guid tarefaId);
    public Task<RequestResponse> ListTasks();
    public Task<RequestResponse> GetTaskById(Guid idTarefa);
}