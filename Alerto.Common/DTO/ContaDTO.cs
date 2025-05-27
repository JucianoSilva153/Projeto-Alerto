using System.Text.Json.Serialization;

namespace Alerto.Common.DTO;

public class AdicionarEditarContaDTO
{
    public string Nome { get; set; }
    public string Email { get; set; }
    public int Contacto { get; set; }
    public string Password { get; set; }
    public string? GoogleId { get; set; }
}

public class AutenticarContaDTO
{
    public string Email { get; set; }
    public string Password { get; set; }
}

public class RetornarContaDTO
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public int? Contacto { get; set; }
}

public class ContaLogadaDTO()
{
    [JsonPropertyName("Id")]
    public int Id { get; set; }
    [JsonPropertyName("Nome")]
    public string Nome { get; set; }
    [JsonPropertyName("Email")]
    public string Email { get; set; }
    [JsonPropertyName("Contacto")]
    public int Contacto { get; set; }
}