using Inventario.Domain.Entities;

namespace Inventario.Domain.Interfaces;

public interface IActivoRepository
{
    Task<IReadOnlyList<Activo>> ObtenerTodosAsync(CancellationToken ct = default);
    Task<IReadOnlyList<Activo>> ObtenerPorSistemaAsync(int sistemaId, CancellationToken ct = default);
    Task<Activo?> ObtenerPorIdAsync(int id, bool incluirRelaciones = true, CancellationToken ct = default);
    Task<Activo> AgregarAsync(Activo activo, CancellationToken ct = default);
    void Actualizar(Activo activo);
    void Eliminar(Activo activo);
    Task<bool> CodigoExisteAsync(string codigo, int? excluirId = null, CancellationToken ct = default);
    Task<int> ContarAsync(CancellationToken ct = default);
}
