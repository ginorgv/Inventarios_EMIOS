using Inventario.Domain.Entities;
using Inventario.Domain.Interfaces;
using Inventario.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Inventario.Infrastructure.Repositories;

public class ClienteDatosRepository : IClienteDatosRepository
{
    private readonly InventarioDbContext _context;

    public ClienteDatosRepository(InventarioDbContext context)
    {
        _context = context;
    }

    public Task<IReadOnlyList<ClienteDatos>> ObtenerTodosAsync(CancellationToken ct = default)
        => _context.ClientesDatos
            .OrderBy(c => c.Id)
            .ToListAsync(ct)
            .ContinueWith(t => (IReadOnlyList<ClienteDatos>)t.Result, ct);

    public Task<ClienteDatos?> ObtenerPorClienteIdAsync(int clienteId, CancellationToken ct = default)
        => _context.ClientesDatos.FirstOrDefaultAsync(c => c.Id == clienteId, ct);

    public async Task<ClienteDatos> AgregarAsync(ClienteDatos datos, CancellationToken ct = default)
    {
        await _context.ClientesDatos.AddAsync(datos, ct);
        return datos;
    }

    public void Actualizar(ClienteDatos datos) => _context.ClientesDatos.Update(datos);
}
