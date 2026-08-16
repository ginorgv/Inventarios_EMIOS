namespace Inventario.Domain.Entities;

/// <summary>
/// Nivel 3 de la jerarquía: Sistema (nuevo, se gestiona en la nueva BD emios_inventario).
/// Hace referencia a una Localización de la BD heredada mediante LocalizacionId
/// (sin FK real en base de datos: la unión se hace en la capa de aplicación).
/// </summary>
public class Sistema : AuditableEntity
{
    public int Id { get; set; }

    public string Codigo { get; set; } = string.Empty;

    public string Nombre { get; set; } = string.Empty;

    public string? Descripcion { get; set; }

    /// <summary>Id de la Localización en emios301 (vista de solo lectura).</summary>
    public int LocalizacionId { get; set; }

    public bool Activo { get; set; } = true;

    public ICollection<Activo> Activos { get; set; } = new List<Activo>();
}
