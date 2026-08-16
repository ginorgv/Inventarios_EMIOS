namespace Inventario.Domain.Entities;

/// <summary>
/// Usuario de la aplicación de inventario (nueva BD emios_inventario).
/// No confundir con la tabla <c>usuario</c> de emios301, que se usa para autenticación
/// y está modelada en <see cref="Usuario"/>.
/// </summary>
public class UsuarioInventario : AuditableEntity
{
    public int Id { get; set; }

    public string NombreUsuario { get; set; } = string.Empty;

    public string NombreCompleto { get; set; } = string.Empty;

    public string? Email { get; set; }

    public string Rol { get; set; } = "Consulta"; // Administrador | Editor | Consulta

    public bool Activo { get; set; } = true;
}
