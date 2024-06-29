namespace Todo.Models;

public class RequestResponse
{
    public bool Sucesso { get; set; }
    public string Mensagem { get; set; }
    public object Target { get; set; } = null;
}
