using Inventario.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventario.Infrastructure.Persistence.Configurations;

public class SistemaConfiguration : IEntityTypeConfiguration<Sistema>
{
    public void Configure(EntityTypeBuilder<Sistema> builder)
    {
        builder.ToTable("sistemas");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Codigo).HasMaxLength(50).IsRequired();
        builder.Property(s => s.Nombre).HasMaxLength(200).IsRequired();
        builder.Property(s => s.Descripcion).HasMaxLength(1000);

        // La localización vive en emios301: se guarda el id como valor plano, sin FK.
        builder.Property(s => s.LocalizacionId).IsRequired();

        builder.HasIndex(s => s.Codigo).IsUnique();

        builder.HasMany(s => s.Activos)
            .WithOne(a => a.Sistema)
            .HasForeignKey(a => a.SistemaId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
