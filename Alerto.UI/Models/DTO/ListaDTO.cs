using Alerto.Common.DTO;

namespace Todo.Models.DTO;

public class TarefasDeUmaListaDTO
{
    public string Lista { get; set; }
    public List<ListaTarefaDTO> Tarefas { get; set; }
}

public class ListaAlteraListaTarefaDTO
{
    public Guid Id { get; set; }
    public string Lista { get; set; }
    public int NumeroTarefas { get; set; }
}

public class CriaListaTarefasDTO
{
    public string Lista { get; set; }
    public int ContaID { get; set; }
}