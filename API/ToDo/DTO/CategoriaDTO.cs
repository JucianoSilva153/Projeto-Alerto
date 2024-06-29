namespace API.ToDo.DTO;


public class CriaCategoriaDTO
{
    public string Categoria { get; set; }
}

public class ListaTarefasPorCategoria
{
    public string Categoria { get; set; }
    public List<ListaTarefaDTO> Tarefas { get; set; }
}

public class ListaAlteraCategorias
{
    public int Id { get; set; }
    public string Categoria { get; set; }
    public int NumeroTarefas { get; set; }
}

