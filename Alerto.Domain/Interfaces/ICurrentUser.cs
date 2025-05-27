namespace Alerto.Domain.Interfaces;

public interface ICurrentUser
{
    Guid ContaId { get; }
    string Email { get; }
    string Contacto { get; }
}