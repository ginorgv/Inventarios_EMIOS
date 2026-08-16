using Inventario.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventario.Infrastructure.Persistence.Configurations;

public class MovimientoConfiguration : IEntityTypeConfiguration<Movimiento>
{
    public void Configure(EntityTypeBuilder<Movimiento> builder)
    {
        builder.ToTable("movimientos");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.Tipo).HasMaxLength(30).IsRequired();
        builder.Property(m => m.Origen).HasMaxLength(255);
        builder.Property(m => m.Destino).HasMaxLength(255);
        builder.Property(m => m.Usuario).HasMaxLength(100);
        builder.Property(m => m.Observaciones).HasMaxLength(2000);

        builder.HasIndex(m => m.ActivoId);
    }
}
