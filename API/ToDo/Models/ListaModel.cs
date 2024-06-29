namespace API.ToDo;

public class ListaModel
{
    public int Id { get; set; }
    public string Nome { get; set; }
    
    
    public int ContaId { get; set; }
    public ContaModel Conta { get; set; }
    
    public List<TarefaModel>? Tarefas { get; set; }
}