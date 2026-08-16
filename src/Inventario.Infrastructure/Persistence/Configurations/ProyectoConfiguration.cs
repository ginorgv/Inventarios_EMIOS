using Inventario.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventario.Infrastructure.Persistence.Configurations;

public class ClienteDatosConfiguration : IEntityTypeConfiguration<ClienteDatos>
{
    public void Configure(EntityTypeBuilder<ClienteDatos> builder)
    {
        builder.ToTable("cliente_datos");

        builder.HasKey(p => p.Id);

        // El Id NO es auto-incremento: es el id del cliente en emios301 (relación 1:1).
        builder.Property(p => p.Id).ValueGeneratedNever();

        builder.Property(p => p.ContractRef).HasColumnName("contract_ref").HasMaxLength(100);
        builder.Property(p => p.ProjectType).HasColumnName("project_type").HasMaxLength(30);
        builder.Property(p => p.StartDate).HasColumnName("start_date");
        builder.Property(p => p.EndDate).HasColumnName("end_date");
    }
}
