using Alerto.Common.Abstractions;
using Alerto.Common.DTO;

namespace Alerto.Domain.Interfaces;

public interface IListasRepository
{
    public Task<RequestResponse> CreateTaskList(CriaListaTarefasDTO lista);
    public Task<RequestResponse> ListTaskList();
    public Task<RequestResponse> ListTasksPerList();
}