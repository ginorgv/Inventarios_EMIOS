namespace Inventario.Domain.Entities;

/// <summary>
/// Documento adjunto a un elemento de la jerarquía (activo, sistema o localización).
/// El fichero se guarda en el almacenamiento (ServicioAlmacenamiento).
/// </summary>
public class Documento : AuditableEntity
{
    public int Id { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public string? Descripcion { get; set; }

    public string TipoDocumento { get; set; } = "General";

    public string ContentType { get; set; } = "application/octet-stream";

    public long TamanoBytes { get; set; }

    /// <summary>Ruta o clave dentro del almacenamiento de ficheros.</summary>
    public string RutaAlmacenamiento { get; set; } = string.Empty;

    /// <summary>Entidad a la que pertenece: Sistema, Activo o Localizacion.</summary>
    public string EntidadTipo { get; set; } = "Activo";

    public int EntidadId { get; set; }

    /// <summary>Id del activo asociado (si aplica).</summary>
    public int? ActivoId { get; set; }

    public string? UsuarioSubio { get; set; }

    public DateTime FechaSubida { get; set; } = DateTime.UtcNow;
}
