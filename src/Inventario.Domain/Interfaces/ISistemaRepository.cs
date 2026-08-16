using Inventario.Domain.Entities;

namespace Inventario.Domain.Interfaces;

public interface ISistemaRepository
{
    Task<IReadOnlyList<Sistema>> ObtenerTodosAsync(CancellationToken ct = default);
    Task<IReadOnlyList<Sistema>> ObtenerPorLocalizacionAsync(int localizacionId, CancellationToken ct = default);
    Task<Sistema?> ObtenerPorIdAsync(int id, CancellationToken ct = default);
    Task<Sistema> AgregarAsync(Sistema sistema, CancellationToken ct = default);
    void Actualizar(Sistema sistema);
    void Eliminar(Sistema sistema);
    Task<bool> CodigoExisteAsync(string codigo, int? excluirId = null, CancellationToken ct = default);
}
