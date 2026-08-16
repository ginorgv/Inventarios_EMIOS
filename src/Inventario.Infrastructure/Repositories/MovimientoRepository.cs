using Inventario.Domain.Entities;
using Inventario.Domain.Interfaces;
using Inventario.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Inventario.Infrastructure.Repositories;

public class MovimientoRepository : IMovimientoRepository
{
    private readonly InventarioDbContext _context;

    public MovimientoRepository(InventarioDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<Movimiento>> ObtenerTodosAsync(CancellationToken ct = default)
        => await _context.Movimientos
            .Include(m => m.Activo)
            .OrderByDescending(m => m.Fecha)
            .ToListAsync(ct);

    public Task<IReadOnlyList<Movimiento>> ObtenerPorActivoAsync(int activoId, CancellationToken ct = default)
        => _context.Movimientos
            .Where(m => m.ActivoId == activoId)
            .OrderByDescending(m => m.Fecha)
            .ToListAsync(ct)
            .ContinueWith(t => (IReadOnlyList<Movimiento>)t.Result, ct);

    public async Task<Movimiento> AgregarAsync(Movimiento movimiento, CancellationToken ct = default)
    {
        await _context.Movimientos.AddAsync(movimiento, ct);
        return movimiento;
    }

    public void Eliminar(Movimiento movimiento) => _context.Movimientos.Remove(movimiento);
}
