using Inventario.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Inventario.Infrastructure.Persistence;

/// <summary>
/// Contexto de ESCRITURA: base de datos nueva <c>emios_inventario</c>.
/// Contiene los niveles 3-5 (Sistema, Activo, Componente), documentos, mantenimientos,
/// movimientos y usuarios de la aplicación. El esquema se gestiona con migraciones EF.
/// </summary>
public class InventarioDbContext : DbContext
{
    public InventarioDbContext(DbContextOptions<InventarioDbContext> options)
        : base(options)
    {
    }

    public DbSet<Sistema> Sistemas => Set<Sistema>();
    public DbSet<Activo> Activos => Set<Activo>();
    public DbSet<Componente> Componentes => Set<Componente>();
    public DbSet<Documento> Documentos => Set<Documento>();
    public DbSet<Mantenimiento> Mantenimientos => Set<Mantenimiento>();
    public DbSet<Movimiento> Movimientos => Set<Movimiento>();
    public DbSet<UsuarioInventario> UsuariosInventario => Set<UsuarioInventario>();

    /// <summary>Datos ampliados del Cliente (1:1 con emios301.clientes mediante Id).</summary>
    public DbSet<ClienteDatos> ClientesDatos => Set<ClienteDatos>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(InventarioDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
