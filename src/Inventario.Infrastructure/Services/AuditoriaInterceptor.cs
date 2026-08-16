using Inventario.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Inventario.Infrastructure.Services;

/// <summary>
/// Rellena automáticamente las marcas temporales de auditoría de las entidades
/// <see cref="AuditableEntity"/> al guardar cambios en emios_inventario.
/// </summary>
public class AuditoriaInterceptor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        AplicarAuditoria(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        AplicarAuditoria(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private static void AplicarAuditoria(DbContext? context)
    {
        if (context is null)
            return;

        var ahora = DateTime.UtcNow;
        foreach (var entrada in context.ChangeTracker.Entries<AuditableEntity>())
        {
            switch (entrada.State)
            {
                case EntityState.Added:
                    entrada.Entity.CreadoEn = ahora;
                    entrada.Entity.ModificadoEn = null;
                    break;
                case EntityState.Modified:
                    entrada.Entity.ModificadoEn = ahora;
                    break;
            }
        }
    }
}
