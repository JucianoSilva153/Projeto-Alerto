namespace Alerto.Domain.Entities;

public class Lista
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Nome { get; set; }
    
    
    public Guid ContaId { get; set; }
    public Conta Conta { get; set; }
    
    public List<Tarefa>? Tarefas { get; set; }
}