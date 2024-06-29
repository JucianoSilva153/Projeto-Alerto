namespace API.ToDo.DTO;

public class CriarNotificacaoDTO
{
    public TarefaModel Tarefa { get; set; }
    public DateTime NotificarAos { get; set; }
}

public class NotificarDTO
{
    public TarefaModel Tarefa { get; set; }
}