namespace Alerto.Common.Abstractions;

public class RequestResponse
{
    public bool Sucesso { get; set; }
    public string Mensagem { get; set; }
    public object Target { get; set; } = null;
}