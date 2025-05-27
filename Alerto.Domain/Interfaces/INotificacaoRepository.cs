using Alerto.Common.Abstractions;
using Alerto.Common.DTO;

namespace Alerto.Domain.Interfaces;

public interface INotificacaoRepository
{
    public Task<RequestResponse> RegisterNotifcation(CriarNotificacaoDTO notificacao);
    public Task<RequestResponse> GetUserNotifications();
    public Task<RequestResponse> ChangeUserNotificationStatus(EstadoNotificacaoDTO estado);
}