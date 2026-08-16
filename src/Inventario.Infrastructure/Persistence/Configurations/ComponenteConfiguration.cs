using Inventario.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventario.Infrastructure.Persistence.Configurations;

public class ComponenteConfiguration : IEntityTypeConfiguration<Componente>
{
    public void Configure(EntityTypeBuilder<Componente> builder)
    {
        builder.ToTable("componentes");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Codigo).HasMaxLength(50).IsRequired();
        builder.Property(c => c.Nombre).HasMaxLength(200).IsRequired();
        builder.Property(c => c.Tipo).HasMaxLength(100);
        builder.Property(c => c.Descripcion).HasMaxLength(2000);

        builder.HasIndex(c => c.ActivoId);

        // Un sensor de emios301 solo puede estar representado por un único Componente.
        builder.HasIndex(c => c.SensorId).IsUnique();

        // Value object: rango de medición → tres columnas propias.
        builder.OwnsOne(c => c.RangoMedicion, rango =>
        {
            rango.Property(r => r.Minimo).HasColumnName("rango_minimo");
            rango.Property(r => r.Maximo).HasColumnName("rango_maximo");
            rango.Property(r => r.Unidad).HasColumnName("rango_unidad").HasMaxLength(20);
        });
    }
}
