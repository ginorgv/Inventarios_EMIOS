using Inventario.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventario.Infrastructure.Persistence.Configurations;

public class MantenimientoConfiguration : IEntityTypeConfiguration<Mantenimiento>
{
    public void Configure(EntityTypeBuilder<Mantenimiento> builder)
    {
        builder.ToTable("mantenimientos");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.Tipo).HasMaxLength(30).IsRequired();
        builder.Property(m => m.Descripcion).HasMaxLength(2000);
        builder.Property(m => m.Estado).HasMaxLength(30).IsRequired();
        builder.Property(m => m.Responsable).HasMaxLength(100);
        builder.Property(m => m.Costo).HasPrecision(14, 2);

        builder.HasIndex(m => m.ActivoId);
    }
}
