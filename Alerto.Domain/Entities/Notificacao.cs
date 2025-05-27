using Alerto.Common.Enums;

namespace Alerto.Domain.Entities;

public class Notificacao
{
    public Guid Id { get; set; }
    public DateTime CriadoAos { get; } = DateTime.Now;
    public EstadoNotificacao Estado { get; set; } = EstadoNotificacao.NotVisualized;
    public DateTime NotificarAos { get; set; }
    public bool Visualizada { get; set; } = false;
    
    public Guid ContaId { get; set; }
    public Conta Conta { get; set; }
    
    public Guid TarefaId { get; set; }
    public Tarefa Tarefa { get; set; }
}