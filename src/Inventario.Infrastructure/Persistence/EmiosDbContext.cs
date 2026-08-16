using Inventario.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Inventario.Infrastructure.Persistence;

/// <summary>
/// Contexto de SOLO LECTURA de la base de datos heredada <c>emios301</c>.
/// NO se crean migraciones ni se modifica su esquema.
/// </summary>
public class EmiosDbContext : DbContext
{
    public EmiosDbContext(DbContextOptions<EmiosDbContext> options)
        : base(options)
    {
        // Nunca se debe guardar cambios en la BD heredada.
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }

    // Guardián de SOLO LECTURA: emios301 no se modifica desde esta aplicación.
    // Si algún código intentara guardar cambios, se lanza una excepción.
    public override int SaveChanges() =>
        throw new NotSupportedException("emios301 es de solo lectura desde esta aplicación.");

    public override int SaveChanges(bool acceptAllChangesOnSuccess) =>
        throw new NotSupportedException("emios301 es de solo lectura desde esta aplicación.");

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        throw new NotSupportedException("emios301 es de solo lectura desde esta aplicación.");

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default) =>
        throw new NotSupportedException("emios301 es de solo lectura desde esta aplicación.");

    public DbSet<Red> Redes => Set<Red>();
    public DbSet<Localizacion> Localizaciones => Set<Localizacion>();

    /// <summary>Tabla <c>clientes</c> de emios301 (Nivel 1, solo lectura).</summary>
    public DbSet<Cliente> Clientes => Set<Cliente>();

    /// <summary>Tabla <c>redes_usuarios</c> de emios301 (asignación usuario → red, solo lectura).</summary>
    public DbSet<RedUsuario> RedesUsuarios => Set<RedUsuario>();

    /// <summary>Tabla <c>usuario</c> de emios301, usada para autenticación.</summary>
    public DbSet<Usuario> Usuarios => Set<Usuario>();

    /// <summary>
    /// Tabla <c>sensores</c> de emios301 (solo lectura). Los sensores se crean en la
    /// otra aplicación (webemios); en esta app se representan como Componentes (nivel 5)
    /// vinculados por <see cref="Componente.SensorId"/>.
    /// </summary>
    public DbSet<Sensor> Sensores => Set<Sensor>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Red>(e =>
        {
            e.ToTable("redes");
            e.HasKey(x => x.Id);
            e.Property(x => x.Nombre).HasColumnName("nombre").HasMaxLength(200);
            e.Property(x => x.ClienteId).HasColumnName("cliente");
        });

        modelBuilder.Entity<Cliente>(e =>
        {
            e.ToTable("clientes");
            e.HasKey(x => x.Id);
            e.Property(x => x.Nombre).HasColumnName("nombre").HasMaxLength(200);
        });

        modelBuilder.Entity<RedUsuario>(e =>
        {
            e.ToTable("redes_usuarios");
            e.HasKey(x => new { x.Usuario, x.RedId });
            e.Property(x => x.Usuario).HasColumnName("usuario").HasMaxLength(100);
            e.Property(x => x.RedId).HasColumnName("red");
        });

        modelBuilder.Entity<Localizacion>(e =>
        {
            e.ToTable("localizaciones");
            e.HasKey(x => x.Id);
            e.Property(x => x.Nombre).HasColumnName("nombre").HasMaxLength(200);
            e.Property(x => x.Descripcion).HasColumnName("descripcion").HasMaxLength(300);
            e.Property(x => x.RedId).HasColumnName("red");
            e.Property(x => x.Latitud).HasColumnName("latitud_mapa_defecto");
            e.Property(x => x.Longitud).HasColumnName("longitud_mapa_defecto");

            // La navegación hacia Sistema pertenece a la BD de escritura
            // (emios_inventario); no debe descubrirse en el modelo de solo lectura.
            e.Ignore(x => x.Sistemas);
        });

        // Entidades que pertenecen a la BD de escritura; nunca se mapean aquí.
        modelBuilder.Ignore<Sistema>();
        modelBuilder.Ignore<Activo>();
        modelBuilder.Ignore<Componente>();
        modelBuilder.Ignore<Documento>();
        modelBuilder.Ignore<Mantenimiento>();
        modelBuilder.Ignore<Movimiento>();
        modelBuilder.Ignore<UsuarioInventario>();

        modelBuilder.Entity<Usuario>(e =>
        {
            e.ToTable("usuarios");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id").HasMaxLength(100);
            e.Property(x => x.PasswordHash).HasColumnName("contrasenya").HasMaxLength(200);
            e.Property(x => x.Nombre).HasColumnName("nombre").HasMaxLength(200);
            e.Property(x => x.Perfil).HasColumnName("perfil").HasMaxLength(100);
        });

        modelBuilder.Entity<Sensor>(e =>
        {
            e.ToTable("sensores");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id");
            e.Property(x => x.Nombre).HasColumnName("nombre").HasMaxLength(200);
            e.Property(x => x.RedId).HasColumnName("red");
            e.Property(x => x.Descripcion).HasColumnName("descripcion").HasMaxLength(3000);
            e.Property(x => x.Tipo).HasColumnName("tipo").HasMaxLength(100);
            e.Property(x => x.LocalizacionId).HasColumnName("localizacion");
            e.Property(x => x.Clase).HasColumnName("clase").HasMaxLength(100);
            e.Property(x => x.TipoValores).HasColumnName("tipo_valores").HasMaxLength(100);
            e.Property(x => x.Calibracion).HasColumnName("calibracion").HasMaxLength(100);
            e.Property(x => x.HoraUltimosValores).HasColumnName("hora_ultimos_valores");
            e.Property(x => x.UltimosValores).HasColumnName("ultimos_valores").HasMaxLength(200);
        });

        base.OnModelCreating(modelBuilder);
    }
}
