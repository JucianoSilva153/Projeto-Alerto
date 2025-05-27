using Alerto.Common.Enums;

namespace Alerto.Common.DTO;

public class CriarNotificacaoDTO
{
    public ListaTarefaDTO Tarefa { get; set; }
    public DateTime NotificarAos { get; set; }
}

public class EstadoNotificacaoDTO
{
    public int Id { get; set; }
    public EstadoNotificacao Estado { get; set; }
}

public class NotificarDTO
{
    public ListaTarefaDTO Tarefa { get; set; }
    public DateTime Data { get; set; }
}