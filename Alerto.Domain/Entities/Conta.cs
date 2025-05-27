namespace Alerto.Domain.Entities;

public class Conta
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Nome { get; set; }
    public string email { get; set; }
    public int? contacto { get; set; }
    public string password { get; set; }
    
    public string? GoogleId { get; set; }

    public List<Lista>? Listas { get; set; }
    public List<Tarefa>? Tarefas { get; set; }
    public List<Categoria>? Categorias { get; set; }
    public List<Notificacao>? Notificacoes { get; set; }
}