using Inventario.Domain.ValueObjects;

namespace Inventario.Domain.Entities;

/// <summary>
/// Nivel 5 de la jerarquía: Componente / Sensor (nuevo, en la BD emios_inventario).
/// </summary>
public class Componente : AuditableEntity
{
    public int Id { get; set; }

    public string Codigo { get; set; } = string.Empty;

    public string Nombre { get; set; } = string.Empty;

    public string? Tipo { get; set; }

    public string? Descripcion { get; set; }

    /// <summary>Rango de medición aplicable cuando el componente es un sensor.</summary>
    public RangoMedicion? RangoMedicion { get; set; }

    public int ActivoId { get; set; }

    public Activo? Activo { get; set; }

    /// <summary>
    /// Id del sensor en <c>emios301.sensores</c> (solo lectura, creado en la otra
    /// aplicación webemios). Los sensores se representan como Componentes del nivel 5;
    /// este enlace permite identificarlos y re-sincronizarlos. Null cuando el componente
    /// no es un sensor (p. ej. creado a mano).
    /// </summary>
    public int? SensorId { get; set; }
}
