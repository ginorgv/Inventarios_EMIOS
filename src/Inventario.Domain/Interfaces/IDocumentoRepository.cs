using Inventario.Domain.Entities;

namespace Inventario.Domain.Interfaces;

public interface IDocumentoRepository
{
    Task<IReadOnlyList<Documento>> ObtenerPorEntidadAsync(string entidadTipo, int entidadId, CancellationToken ct = default);
    Task<Documento?> ObtenerPorIdAsync(int id, CancellationToken ct = default);
    Task<Documento> AgregarAsync(Documento documento, CancellationToken ct = default);
    void Eliminar(Documento documento);
}
