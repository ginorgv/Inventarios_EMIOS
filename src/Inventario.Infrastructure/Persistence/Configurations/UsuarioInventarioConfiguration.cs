using Inventario.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventario.Infrastructure.Persistence.Configurations;

public class UsuarioInventarioConfiguration : IEntityTypeConfiguration<UsuarioInventario>
{
    public void Configure(EntityTypeBuilder<UsuarioInventario> builder)
    {
        builder.ToTable("usuarios_inventario");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.NombreUsuario).HasMaxLength(100).IsRequired();
        builder.Property(u => u.NombreCompleto).HasMaxLength(200).IsRequired();
        builder.Property(u => u.Email).HasMaxLength(200);
        builder.Property(u => u.Rol).HasMaxLength(30).IsRequired();

        builder.HasIndex(u => u.NombreUsuario).IsUnique();
    }
}
