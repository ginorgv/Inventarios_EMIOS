namespace Inventario.Domain.Entities;

public class Mantenimiento : AuditableEntity
{
    public int Id { get; set; }

    public int ActivoId { get; set; }

    public Activo? Activo { get; set; }

    public string Tipo { get; set; } = "Preventivo"; // Preventivo | Correctivo | Predictivo

    public DateTime? FechaProgramada { get; set; }

    public DateTime? FechaEjecucion { get; set; }

    public string? Descripcion { get; set; }

    public decimal? Costo { get; set; }

    public string? Responsable { get; set; }

    public string Estado { get; set; } = "Programado"; // Programado | EnCurso | Completado | Cancelado
}
