namespace Alerto.Common.DTO;

public class ModoChatoDTO
{
    public Guid TarefaId { get; set; }
    public string NomeTarefa { get; set; }
    public DateTime DataCriacao { get; set; }
    public DateTime DataTermino { get; set; }
    public int DiasAntes { get; set; }
    public int FrequenciaAlerta { get; set; }
}