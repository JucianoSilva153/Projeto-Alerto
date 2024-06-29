namespace API.ToDo;

public class ContaModel
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string email { get; set; }
    public int contacto { get; set; }
    public string password { get; set; }

    public List<ListaModel>? Listas { get; set; }
    public List<TarefaModel>? Tarefas { get; set; }
    public List<CategoriaModel>? Categorias { get; set; }
    public List<NotificacaoModel>? Notificacoes { get; set; }
}