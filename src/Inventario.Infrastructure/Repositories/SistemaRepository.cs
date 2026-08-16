using Inventario.Domain.Entities;
using Inventario.Domain.Interfaces;
using Inventario.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Inventario.Infrastructure.Repositories;

public class SistemaRepository : ISistemaRepository
{
    private readonly InventarioDbContext _context;

    public SistemaRepository(InventarioDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<Sistema>> ObtenerTodosAsync(CancellationToken ct = default)
        => await _context.Sistemas
            .Include(s => s.Activos)
            .OrderBy(s => s.Nombre)
            .ToListAsync(ct);

    public async Task<IReadOnlyList<Sistema>> ObtenerPorLocalizacionAsync(int localizacionId, CancellationToken ct = default)
        => await _context.Sistemas
            .Where(s => s.LocalizacionId == localizacionId)
            .Include(s => s.Activos)
            .OrderBy(s => s.Nombre)
            .ToListAsync(ct);

    public async Task<Sistema?> ObtenerPorIdAsync(int id, CancellationToken ct = default)
        => await _context.Sistemas
            .Include(s => s.Activos)
            .FirstOrDefaultAsync(s => s.Id == id, ct);

    public async Task<Sistema> AgregarAsync(Sistema sistema, CancellationToken ct = default)
    {
        await _context.Sistemas.AddAsync(sistema, ct);
        return sistema;
    }

    public void Actualizar(Sistema sistema) => _context.Sistemas.Update(sistema);

    public void Eliminar(Sistema sistema) => _context.Sistemas.Remove(sistema);

    public Task<bool> CodigoExisteAsync(string codigo, int? excluirId = null, CancellationToken ct = default)
    {
        var query = _context.Sistemas.Where(s => s.Codigo == codigo);
        if (excluirId.HasValue)
            query = query.Where(s => s.Id != excluirId.Value);
        return query.AnyAsync(ct);
    }
}
