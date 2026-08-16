using Inventario.Domain.Entities;
using Inventario.Domain.Interfaces;
using Inventario.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Inventario.Infrastructure.Repositories;

public class MantenimientoRepository : IMantenimientoRepository
{
    private readonly InventarioDbContext _context;

    public MantenimientoRepository(InventarioDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<Mantenimiento>> ObtenerTodosAsync(CancellationToken ct = default)
        => await _context.Mantenimientos
            .Include(m => m.Activo)
            .OrderByDescending(m => m.FechaProgramada)
            .ToListAsync(ct);

    public Task<IReadOnlyList<Mantenimiento>> ObtenerPorActivoAsync(int activoId, CancellationToken ct = default)
        => _context.Mantenimientos
            .Where(m => m.ActivoId == activoId)
            .OrderByDescending(m => m.FechaProgramada)
            .ToListAsync(ct)
            .ContinueWith(t => (IReadOnlyList<Mantenimiento>)t.Result, ct);

    public async Task<Mantenimiento> AgregarAsync(Mantenimiento mantenimiento, CancellationToken ct = default)
    {
        await _context.Mantenimientos.AddAsync(mantenimiento, ct);
        return mantenimiento;
    }

    public void Eliminar(Mantenimiento mantenimiento) => _context.Mantenimientos.Remove(mantenimiento);
}
