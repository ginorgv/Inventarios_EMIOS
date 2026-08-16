using Inventario.Domain.Entities;

namespace Inventario.Domain.Interfaces;

public interface IClienteDatosRepository
{
    Task<IReadOnlyList<ClienteDatos>> ObtenerTodosAsync(CancellationToken ct = default);
    Task<ClienteDatos?> ObtenerPorClienteIdAsync(int clienteId, CancellationToken ct = default);
    Task<ClienteDatos> AgregarAsync(ClienteDatos datos, CancellationToken ct = default);
    void Actualizar(ClienteDatos datos);
}
