using System;
using System.Threading.Tasks;
using Alerto.Common.Abstractions;
using Alerto.Common.DTO;
using Alerto.Domain.Interfaces;

namespace Alerto.Application.Services;

public class NotificacaoService(INotificacaoRepository notificacaoRepository)
{
    public async Task<RequestResponse> RegistarNotificacaoAsync(CriarNotificacaoDTO novaNotificacao)
    {
        try
        {
            return await notificacaoRepository.RegisterNotifcation(novaNotificacao);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Erro ao tentar registar notificao: {e.Message}");
        }

        return new RequestResponse
        {
            Mensagem = "Erro ao tentar registar a notificacao!!",
            Sucesso = false
        };
    }
    
    public async Task<RequestResponse> RetornarNotificacoesAsync()
    {
        try
        {
            return await notificacaoRepository.GetUserNotifications();
        }
        catch (Exception e)
        {
            Console.WriteLine($"Erro ao tentar retornar notificacoes: {e.Message}");
        }

        return new RequestResponse
        {
            Mensagem = "Erro ao tentar retornar notificacoes!!",
            Sucesso = false
        };
    }
    
    public async Task<RequestResponse> MudarEstadoNotificacao(EstadoNotificacaoDTO novoEstado)
    {
        try
        {
            return await notificacaoRepository.ChangeUserNotificationStatus(novoEstado);
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
}