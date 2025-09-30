using Alerto.Common.DTO;
using Quartz;
using Quartz.Impl.Matchers;

namespace Alerto.Infrastructure.Services.ModoChatoService;

public class AlertoService : IAlertoService
{
    private readonly ISchedulerFactory _schedulerFactory;

    public AlertoService(ISchedulerFactory schedulerFactory)
    {
        _schedulerFactory = schedulerFactory;
    }
    
    public async Task AgendarAlarme(ModoChatoDTO modoChato)
    {
        try
        {
            var scheduler = await _schedulerFactory.GetScheduler();

            if (modoChato.DiasAntes is null || modoChato.FrequenciaAlerta is null)
                throw new NullReferenceException();
            
            // Quantos alarmes serão agendados?
            var alarmStart = modoChato.DataTermino.AddDays((double)-modoChato.DiasAntes);
            var alarmInterval = TimeSpan.FromDays((double)modoChato.FrequenciaAlerta);
            var alarmCount = (modoChato.DiasAntes / modoChato.FrequenciaAlerta) + 1;

            // Quando agendas:
            string group = $"Tarefa_{modoChato.TarefaId}";

            for (int i = 0; i < alarmCount; i++)
            {
                var alarmTime = alarmStart.AddDays(i * (double)modoChato.FrequenciaAlerta);
                var diasRestantes = (modoChato.DataTermino - alarmTime).Days;

                var job = JobBuilder.Create<ModoChatoAlerto>()
                    .WithIdentity($"Alerto_{modoChato.TarefaId}_{i}", group)
                    .UsingJobData("TarefaId", modoChato.TarefaId)
                    .UsingJobData("DiasRestantes", diasRestantes)
                    .Build();

                var trigger = TriggerBuilder.Create()
                    .StartAt(alarmTime)
                    .WithSimpleSchedule(s => s.WithRepeatCount(0))
                    .Build();

                await scheduler.ScheduleJob(job, trigger);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    public async Task CancelarAlarmes(Guid tarefaId)
    {
        try
        {
            var scheduler = await _schedulerFactory.GetScheduler();
            string group = $"Tarefa_{tarefaId}";
            await scheduler.DeleteJobs((await scheduler.GetJobKeys(GroupMatcher<JobKey>.GroupEquals(group))).ToList()); 
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}