namespace Alerto.Domain.Entities;

public class Tarefa
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string? Nome { get; set; }
    public string? Prioridade { get; set; }
    public string? Descricao { get; set; }
    public string? DataConclusao { get; set; }
    public string? DataCriacao { get; set; }
    public bool Concluida { get; set; } = false;
    
    
    public Guid ContaId { get; set; }
    public Conta Conta { get; set; }
     
    public Guid CategoriaId { get; set; }
    public Categoria? Categoria { get; set; }

    public Guid ListaId { get; set; }
    
    public Lista? Lista { get; set; }
}