using Inventario.Domain.Entities;
using Inventario.Domain.Interfaces;
using Inventario.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Inventario.Infrastructure.Repositories;

public class ActivoRepository : IActivoRepository
{
    private readonly InventarioDbContext _context;

    public ActivoRepository(InventarioDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<Activo>> ObtenerTodosAsync(CancellationToken ct = default)
        => await _context.Activos
            .Include(a => a.Componentes)
            .Include(a => a.Sistema)
            .OrderBy(a => a.Nombre)
            .ToListAsync(ct);

    public async Task<IReadOnlyList<Activo>> ObtenerPorSistemaAsync(int sistemaId, CancellationToken ct = default)
        => await _context.Activos
            .Where(a => a.SistemaId == sistemaId)
            .Include(a => a.Componentes)
            .OrderBy(a => a.Nombre)
            .ToListAsync(ct);

    public async Task<Activo?> ObtenerPorIdAsync(int id, bool incluirRelaciones = true, CancellationToken ct = default)
    {
        var query = _context.Activos.AsQueryable();
        if (incluirRelaciones)
        {
            query = query
                .Include(a => a.Componentes)
                .Include(a => a.Documentos)
                .Include(a => a.Mantenimientos)
                .Include(a => a.Movimientos)
                .Include(a => a.Sistema);
        }

        return await query.FirstOrDefaultAsync(a => a.Id == id, ct);
    }

    public async Task<Activo> AgregarAsync(Activo activo, CancellationToken ct = default)
    {
        await _context.Activos.AddAsync(activo, ct);
        return activo;
    }

    public void Actualizar(Activo activo) => _context.Activos.Update(activo);

    public void Eliminar(Activo activo) => _context.Activos.Remove(activo);

    public Task<bool> CodigoExisteAsync(string codigo, int? excluirId = null, CancellationToken ct = default)
    {
        var query = _context.Activos.Where(a => a.Codigo == codigo);
        if (excluirId.HasValue)
            query = query.Where(a => a.Id != excluirId.Value);
        return query.AnyAsync(ct);
    }

    public Task<int> ContarAsync(CancellationToken ct = default) => _context.Activos.CountAsync(ct);
}
