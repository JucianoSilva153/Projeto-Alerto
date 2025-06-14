using Alerto.Common.DTO;

namespace Alerto.Infrastructure.Services.ModoChatoService;

public interface IAlertoService
{
    public Task AgendarAlarme(ModoChatoDTO modoChato);
    public Task CancelarAlarmes(Guid tarefaId);
}