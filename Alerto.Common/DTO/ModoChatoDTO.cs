namespace Alerto.Common.DTO;

public class ModoChatoDTO
{
    public Guid TarefaId { get; set; }
    public string NomeTarefa { get; set; }
    public DateTime DataCriacao { get; set; }
    public DateTime DataTermino { get; set; } = DateTime.Now.AddDays(3);
    public int? DiasAntes { get; set; }
    public int? FrequenciaAlerta { get; set; }

    public bool IsAtivated { get; set; } = false;
}