namespace API.ToDo;

public class NotificacaoModel
{
    public int Id { get; set; }
    public DateTime CriadoAos { get; } = DateTime.Now;
    public string Estado { get; set; } = "standby";
    public DateTime NotificarAos { get; set; }
    public bool Visualizada { get; set; } = false;
    
    
    public int ContaId { get; set; }
    public ContaModel Conta { get; set; }
    
    public int TarefaId { get; set; }
    public TarefaModel Tarefa { get; set; }
}