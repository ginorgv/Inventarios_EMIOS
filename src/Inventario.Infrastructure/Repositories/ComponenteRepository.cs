using Inventario.Domain.Entities;
using Inventario.Domain.Interfaces;
using Inventario.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Inventario.Infrastructure.Repositories;

public class ComponenteRepository : IComponenteRepository
{
    private readonly InventarioDbContext _context;

    public ComponenteRepository(InventarioDbContext context)
    {
        _context = context;
    }

    public Task<IReadOnlyList<Componente>> ObtenerTodosAsync(CancellationToken ct = default)
        => _context.Componentes
            .OrderBy(c => c.Nombre)
            .ToListAsync(ct)
            .ContinueWith(t => (IReadOnlyList<Componente>)t.Result, ct);

    public Task<IReadOnlyList<Componente>> ObtenerPorActivoAsync(int activoId, CancellationToken ct = default)
        => _context.Componentes
            .Where(c => c.ActivoId == activoId)
            .OrderBy(c => c.Nombre)
            .ToListAsync(ct)
            .ContinueWith(t => (IReadOnlyList<Componente>)t.Result, ct);

    public Task<Componente?> ObtenerPorIdAsync(int id, CancellationToken ct = default)
        => _context.Componentes.FirstOrDefaultAsync(c => c.Id == id, ct);

    public Task<Componente?> ObtenerPorSensorIdAsync(int sensorId, CancellationToken ct = default)
        => _context.Componentes.FirstOrDefaultAsync(c => c.SensorId == sensorId, ct);

    public async Task<Componente> AgregarAsync(Componente componente, CancellationToken ct = default)
    {
        await _context.Componentes.AddAsync(componente, ct);
        return componente;
    }

    public void Actualizar(Componente componente) => _context.Componentes.Update(componente);

    public void Eliminar(Componente componente) => _context.Componentes.Remove(componente);
}
