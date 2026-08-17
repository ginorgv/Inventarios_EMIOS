namespace Inventario.Domain.Entities;

/// <summary>
/// Permiso explícito de un usuario (login) sobre un nodo de la jerarquía.
/// Se guarda en emios_inventario (la BD de escritura). Un permiso sobre un nodo
/// se hereda hacia abajo (su subárbol) y hace visibles sus ancestros como ruta.
/// <c>EntidadTipo</c>: "global" | "cliente" | "localizacion" | "sistema" | "activo".
/// <c>EntidadId</c>: id del nodo; 0 para "global".
/// </summary>
public class Permiso : AuditableEntity
{
    public int Id { get; set; }

    /// <summary>Login del usuario (columna usuario de emios301).</summary>
    public string Usuario { get; set; } = string.Empty;

    public string EntidadTipo { get; set; } = string.Empty;

    public int EntidadId { get; set; }

    public TipoPermiso TipoPermiso { get; set; } = TipoPermiso.Lectura;
}
