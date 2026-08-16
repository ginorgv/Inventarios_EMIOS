namespace Inventario.Domain.Entities;

/// <summary>
/// Sensor del sistema de monitorización EMIOS (tabla <c>sensores</c> de emios301).
/// SOLO LECTURA: los sensores se crean y gestionan en la otra aplicación (webemios).
/// En esta aplicación se representan como <see cref="Componente"/> (nivel 5) mediante
/// <see cref="Componente.SensorId"/>.
/// </summary>
public class Sensor
{
    public int Id { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public int RedId { get; set; }

    public string? Descripcion { get; set; }

    public string? Tipo { get; set; }

    public int LocalizacionId { get; set; }

    public string? Clase { get; set; }

    public string? TipoValores { get; set; }

    public string? Calibracion { get; set; }

    public DateTime? HoraUltimosValores { get; set; }

    public string? UltimosValores { get; set; }
}
