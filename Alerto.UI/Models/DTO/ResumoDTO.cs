namespace Todo.Models.DTO;

public record ResumoDTO()
{
    public int Tarefas { get; set; }
    public int ListasTarefas { get; set; }
    public int Categoria { get; set; }
    public int Concluidas { get; set; }
}