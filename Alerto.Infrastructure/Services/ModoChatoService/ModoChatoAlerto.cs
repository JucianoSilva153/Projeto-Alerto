using Alerto.Persistance.Context;
using Quartz;

namespace Alerto.Infrastructure.Services.ModoChatoService;

public class ModoChatoAlerto : IJob
{
    private readonly  DBToDO _dbContext;

    public ModoChatoAlerto(DBToDO dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var tarefaId = context.JobDetail.JobDataMap.GetGuidValue("TarefaId");
        var nomeTarefa = context.JobDetail.JobDataMap.GetString("Tarefa"); 
        var diasRestantes = context.JobDetail.JobDataMap.GetIntValue("Tarefa");

        var tarefa = await _dbContext.Tarefas.FindAsync(tarefaId);
        if (tarefa == null) return;
        

        // Enviar notificação ou alarme...
        // Por ex.: Enviar um email, ou um push notification
    }
}