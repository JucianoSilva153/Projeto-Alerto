using Alerto.Common.Abstractions;
using Alerto.Common.DTO;

namespace Alerto.Domain.Interfaces;

public interface ICategoriaRepository
{
    public Task<RequestResponse> CreateCategoria(CriaCategoriaDTO categoria);
    public Task<RequestResponse> ListAllCategorias();
    public Task<RequestResponse> EditCategoria(ListaAlteraCategorias categoria);
    public Task<RequestResponse> DeleteCategoria(Guid IdCategoria);
}