namespace Inventario.Domain.Entities;

/// <summary>
/// Datos ampliados del Cliente (Nivel 1). Viven en emios_inventario con relación 1:1
/// a emios301.clientes: el Id ES el id del cliente (sin auto-incremento), de modo que
/// la relación es por construcción. emios301 no se modifica (solo lectura).
/// </summary>
public class ClienteDatos : AuditableEntity
{
    /// <summary>Id del cliente en emios301.clientes (relación 1:1).</summary>
    public int Id { get; set; }

    /// <summary>Referencia de contrato.</summary>
    public string? ContractRef { get; set; }

    /// <summary>solar | eólico | BESS | híbrido | industrial</summary>
    public string? ProjectType { get; set; }

    /// <summary>Fecha de inicio del proyecto.</summary>
    public DateTime? StartDate { get; set; }

    /// <summary>Fecha de fin del proyecto (opcional).</summary>
    public DateTime? EndDate { get; set; }
}
