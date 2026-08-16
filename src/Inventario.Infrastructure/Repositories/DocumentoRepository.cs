using Inventario.Domain.Entities;
using Inventario.Domain.Interfaces;
using Inventario.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Inventario.Infrastructure.Repositories;

public class DocumentoRepository : IDocumentoRepository
{
    private readonly InventarioDbContext _context;

    public DocumentoRepository(InventarioDbContext context)
    {
        _context = context;
    }

    public Task<IReadOnlyList<Documento>> ObtenerPorEntidadAsync(string entidadTipo, int entidadId, CancellationToken ct = default)
        => _context.Documentos
            .Where(d => d.EntidadTipo == entidadTipo && d.EntidadId == entidadId)
            .OrderByDescending(d => d.FechaSubida)
            .ToListAsync(ct)
            .ContinueWith(t => (IReadOnlyList<Documento>)t.Result, ct);

    public Task<Documento?> ObtenerPorIdAsync(int id, CancellationToken ct = default)
        => _context.Documentos.FirstOrDefaultAsync(d => d.Id == id, ct);

    public async Task<Documento> AgregarAsync(Documento documento, CancellationToken ct = default)
    {
        await _context.Documentos.AddAsync(documento, ct);
        return documento;
    }

    public void Eliminar(Documento documento) => _context.Documentos.Remove(documento);
}
