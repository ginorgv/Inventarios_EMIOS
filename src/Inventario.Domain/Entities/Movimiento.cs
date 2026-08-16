namespace Inventario.Domain.Entities;

/// <summary>
/// Movimiento de un activo (entrada, salida, traslado...).
/// </summary>
public class Movimiento : AuditableEntity
{
    public int Id { get; set; }

    public int ActivoId { get; set; }

    public Activo? Activo { get; set; }

    public string Tipo { get; set; } = "Traslado"; // Entrada | Salida | Traslado | Baja

    public DateTime Fecha { get; set; } = DateTime.UtcNow;

    public string? Origen { get; set; }

    public string? Destino { get; set; }

    public string? Usuario { get; set; }

    public string? Observaciones { get; set; }
}
