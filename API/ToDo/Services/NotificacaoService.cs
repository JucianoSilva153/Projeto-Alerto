using API.Context;
using API.ToDo.DTO;
using Microsoft.EntityFrameworkCore;

namespace API.ToDo.Services;

public class NotificacaoService
{
    private DBToDO acessoDados;

    public NotificacaoService(DBToDO acessoDados)
    {
        this.acessoDados = acessoDados;
    }

    public async Task<RequestResponse> RegisterNotifcation(CriarNotificacaoDTO Notificacao, int Id)
    {
        try
        {
            var notificacao = new NotificacaoModel
            {
                ContaId = Id,
                TarefaId = Notificacao.Tarefa.Id,
                NotificarAos = Notificacao.NotificarAos,
            };

            await acessoDados.Notificacao.AddAsync(notificacao);
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

    
}