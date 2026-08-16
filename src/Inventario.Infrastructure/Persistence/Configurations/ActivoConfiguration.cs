using Inventario.Domain.Entities;
using Inventario.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventario.Infrastructure.Persistence.Configurations;

public class ActivoConfiguration : IEntityTypeConfiguration<Activo>
{
    public void Configure(EntityTypeBuilder<Activo> builder)
    {
        builder.ToTable("activos");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Codigo).HasMaxLength(50).IsRequired();
        builder.Property(a => a.Nombre).HasMaxLength(200).IsRequired();
        builder.Property(a => a.Descripcion).HasMaxLength(2000);
        builder.Property(a => a.TipoActivo).HasMaxLength(100);
        builder.Property(a => a.Fabricante).HasMaxLength(100);
        builder.Property(a => a.Modelo).HasMaxLength(100);
        builder.Property(a => a.NumeroSerie).HasMaxLength(100);

        builder.HasIndex(a => a.Codigo).IsUnique();
        builder.HasIndex(a => a.SistemaId);

        builder.Property(a => a.Estado)
            .HasConversion<string>()
            .HasMaxLength(30);

        // Value object: coordenadas → dos columnas propias.
        builder.OwnsOne(a => a.Ubicacion, ubicacion =>
        {
            ubicacion.Property(c => c.Latitud).HasColumnName("ubicacion_latitud").HasPrecision(10, 6);
            ubicacion.Property(c => c.Longitud).HasColumnName("ubicacion_longitud").HasPrecision(10, 6);
        });

        builder.HasMany(a => a.Componentes)
            .WithOne(c => c.Activo)
            .HasForeignKey(c => c.ActivoId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(a => a.Mantenimientos)
            .WithOne(m => m.Activo)
            .HasForeignKey(m => m.ActivoId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(a => a.Movimientos)
            .WithOne(m => m.Activo)
            .HasForeignKey(m => m.ActivoId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
