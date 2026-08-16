namespace Inventario.Domain.Entities;

/// <summary>
/// Red (solo lógica, NUNCA visual): se usa para el filtrado multi-tenant.
/// Vista de SOLO LECTURA de la tabla <c>redes</c> de emios301.
/// </summary>
public class Red
{
    public int Id { get; set; }

    public string Nombre { get; set; } = string.Empty;

    /// <summary>Columna <c>cliente</c> (FK a la tabla clientes).</summary>
    public int ClienteId { get; set; }

    // Navegación de solo lectura (no se persiste en esta BD).
    public ICollection<Localizacion> Localizaciones { get; set; } = new List<Localizacion>();
}
