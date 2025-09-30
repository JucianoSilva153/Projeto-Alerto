namespace Alerto.API.ToDo.Services.OmbalaService.Models;

public record Message
{
    public string message { get; set; } = "";
    public string from { get; set; } = "";
    public string to { get; set; } = "";
    public string? schedule { get; set; }
}