using Alerto.Common.Abstractions;
using Alerto.Common.DTO;
using Alerto.Common.Enums;
using Alerto.Domain.Entities;
using Alerto.Domain.Interfaces;
using Alerto.Persistance.Context;
using Microsoft.EntityFrameworkCore;

namespace Alerto.API.ToDo.Repositories.NotificacaoRepository;

public class NotificacaoRepository(DBToDO acessoDados, ICurrentUser currentUser) : INotificacaoRepository
{
    public async Task<RequestResponse> RegisterNotifcation(CriarNotificacaoDTO notificacao)
    {
        try
        {
            var notif = new Notificacao
            {
                ContaId = currentUser.ContaId,
                TarefaId = notificacao.Tarefa.Id,
                NotificarAos = notificacao.NotificarAos
            };

            await acessoDados.Notificacoes.AddAsync(notif);
            await acessoDados.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new RequestResponse
            {
                Mensagem = "Erro ao registrar Notificacao!",
                Sucesso = false,
                Target = null
            };
        }

        return new RequestResponse
        {
            Mensagem = "Notificacao Registada Com Sucesso",
            Sucesso = true,
            Target = null,
        };
    }

    public async Task<RequestResponse> GetUserNotifications()
    {
        try
        {
            var notificacoes = await acessoDados.Notificacoes
                .Where(n => n.ContaId == currentUser.ContaId && n.Estado == EstadoNotificacao.NotVisualized)
                .Include(notificacao => notificacao.Tarefa)
                .ToListAsync();

            return new RequestResponse
            {
                Mensagem = "Notificacoes",
                Sucesso = true,
                Target = notificacoes.Select(a => new NotificarDTO
                {
                    Tarefa =
                    {
                        Tarefa = a.Tarefa.Nome,
                    }
                })
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public Task<RequestResponse> ChangeUserNotificationStatus(EstadoNotificacaoDTO estado)
    {
        throw new NotImplementedException();
    }
}