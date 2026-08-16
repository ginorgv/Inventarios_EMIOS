namespace Inventario.Domain.Entities;

/// <summary>
/// Nivel 2 de la jerarquía: Instalación / Site (geolocalización).
/// Vista de SOLO LECTURA de la tabla <c>localizaciones</c> de emios301.
/// </summary>
public class Localizacion
{
    public int Id { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public string? Descripcion { get; set; }

    /// <summary>Columna <c>red</c> (FK a la red; solo lógica, nunca visual).</summary>
    public int RedId { get; set; }

    /// <summary>Geolocalización (columnas latitud_mapa_defecto / longitud_mapa_defecto).</summary>
    public double? Latitud { get; set; }

    public double? Longitud { get; set; }

    // Navegación de solo lectura (no se persiste en esta BD).
    public Red? Red { get; set; }

    // Navegación hacia la BD de escritura (no se mapea en emios301).
    public ICollection<Sistema> Sistemas { get; set; } = new List<Sistema>();
}
