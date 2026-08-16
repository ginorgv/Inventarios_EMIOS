namespace Inventario.Domain.Entities;

/// <summary>
/// Base común para las entidades auditables de la nueva BD (emios_inventario).
/// </summary>
public abstract class AuditableEntity
{
    public DateTime CreadoEn { get; set; } = DateTime.UtcNow;

    public string? CreadoPor { get; set; }

    public DateTime? ModificadoEn { get; set; }

    public string? ModificadoPor { get; set; }
}
