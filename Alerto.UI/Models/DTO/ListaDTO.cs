namespace Todo.Models.DTO;

public class CriaListaTarefasDTO
{
    public string Lista { get; set; }
    public int ContaID { get; set; }
}

public class ListaAlteraListaTarefaDTO
{
    public int Id { get; set; }
    public string Lista { get; set; }    
    public int NumeroTarefas { get; set; }
}

public class RetornaListaTarefasDTO
{
    public string Lista { get; set; }
    public int ContaID { get; set; }
}