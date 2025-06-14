namespace Alerto.Infrastructure.Services.ModoChatoService;
/*
public class NotificacaoTaskService : BackgroundService
{
    private PeriodicTimer timer = new(TimeSpan.FromMilliseconds(10000));
    private readonly IServiceScopeFactory _scopeFactory;


    public NotificacaoTaskService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (await timer.WaitForNextTickAsync() && !stoppingToken.IsCancellationRequested)
        {
            var notificar = await VerificaNotificacaoPendente();
            if (notificar)
            {
                Console.WriteLine("Notificacao Para Hoje encontrada!!");
            }
            else
            {
                Console.WriteLine("Notificacao Para Hoje nao encontrada!!");
            }
        }
    }

    public async Task<bool> VerificaNotificacaoPendente()
    {
        try
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var acessoDados = scope.ServiceProvider.GetRequiredService<DBToDO>();
                var lista = await acessoDados.Notificacao
                    .Include(t => t.Tarefa)
                    .ThenInclude(c => c.Conta)
                    .Where(n => n.NotificarAos.Date == DateTime.Now.Date && n.Estado == "standby")
                    .ToListAsync();

                if (lista is not null && lista.Count > 0)
                {
                    foreach (var notificacao in lista)
                    {
                        notificacao.Estado = "pending";
                    }

                    await acessoDados.SaveChangesAsync();
                }
                else
                {
                    return false;
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }

        return true;
    }
}*/