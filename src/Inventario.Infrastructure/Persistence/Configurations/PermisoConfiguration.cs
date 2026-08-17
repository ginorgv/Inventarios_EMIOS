using Inventario.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventario.Infrastructure.Persistence.Configurations;

public class PermisoConfiguration : IEntityTypeConfiguration<Permiso>
{
    public void Configure(EntityTypeBuilder<Permiso> builder)
    {
        builder.ToTable("permisos");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Usuario).HasMaxLength(100).IsRequired();
        builder.Property(p => p.EntidadTipo).HasMaxLength(20).IsRequired();

        // Un usuario solo puede tener un permiso por (tipo, entidad).
        builder.HasIndex(p => new { p.Usuario, p.EntidadTipo, p.EntidadId }).IsUnique();
    }
}
