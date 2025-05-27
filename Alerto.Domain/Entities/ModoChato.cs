namespace Alerto.Domain.Entities;

public class ModoChato
{
    public bool Ativo { get; set; } = false;
    public int DiasParaChatear { get; set; } = 3;
}