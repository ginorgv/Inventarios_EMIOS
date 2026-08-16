using Inventario.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventario.Infrastructure.Persistence.Configurations;

public class DocumentoConfiguration : IEntityTypeConfiguration<Documento>
{
    public void Configure(EntityTypeBuilder<Documento> builder)
    {
        builder.ToTable("documentos");

        builder.HasKey(d => d.Id);

        builder.Property(d => d.Nombre).HasMaxLength(255).IsRequired();
        builder.Property(d => d.Descripcion).HasMaxLength(1000);
        builder.Property(d => d.TipoDocumento).HasMaxLength(100).IsRequired();
        builder.Property(d => d.ContentType).HasMaxLength(200).IsRequired();
        builder.Property(d => d.RutaAlmacenamiento).HasMaxLength(1000).IsRequired();
        builder.Property(d => d.EntidadTipo).HasMaxLength(30).IsRequired();
        builder.Property(d => d.UsuarioSubio).HasMaxLength(100);

        builder.HasIndex(d => new { d.EntidadTipo, d.EntidadId });

        // Sin FK real: los documentos son polimórficos (Activo, Sistema o Localizacion).
    }
}
