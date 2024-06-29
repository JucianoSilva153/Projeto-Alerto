using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace API.ToDo;

public class TarefaModel
{
    public int Id { get; set; }
    public string? Nome { get; set; }
    public string? Prioridade { get; set; }
    public string? Descricao { get; set; }
    public string? DataConclusao { get; set; }
    public string? DataCriacao { get; set; }
    public bool Concluida { get; set; } = false;


    
    public int ContaId { get; set; }
    public ContaModel Conta { get; set; }
     
    public int CategoriaId { get; set; }
    public CategoriaModel? Categoria { get; set; }

    public int ListaId { get; set; }
    
    public ListaModel? Lista { get; set; }
}