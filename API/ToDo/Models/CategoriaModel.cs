namespace API.ToDo;

public class CategoriaModel
{
    public int Id { get; set; }
    public string Nome { get; set; }

    public int ContaId { get; set; }
    public ContaModel Conta { get; set; }
    public ICollection<TarefaModel>? Tarefas { get; set; }
}