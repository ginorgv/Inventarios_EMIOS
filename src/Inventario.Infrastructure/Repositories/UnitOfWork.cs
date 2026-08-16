using Inventario.Domain.Interfaces;
using Inventario.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore.Storage;

namespace Inventario.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly InventarioDbContext _context;
    private IDbContextTransaction? _transaccion;

    public UnitOfWork(InventarioDbContext context)
    {
        _context = context;
    }

    public Task<int> SaveChangesAsync(CancellationToken ct = default)
        => _context.SaveChangesAsync(ct);

    public async Task BeginTransactionAsync(CancellationToken ct = default)
        => _transaccion = await _context.Database.BeginTransactionAsync(ct);

    public async Task CommitAsync(CancellationToken ct = default)
    {
        if (_transaccion is null)
            return;
        await _transaccion.CommitAsync(ct);
        await _transaccion.DisposeAsync();
        _transaccion = null;
    }

    public async Task RollbackAsync(CancellationToken ct = default)
    {
        if (_transaccion is null)
            return;
        await _transaccion.RollbackAsync(ct);
        await _transaccion.DisposeAsync();
        _transaccion = null;
    }
}
