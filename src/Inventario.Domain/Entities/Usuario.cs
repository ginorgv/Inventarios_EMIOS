namespace Inventario.Domain.Entities;

/// <summary>
/// Usuario de la tabla <c>usuarios</c> de la BD heredada EMIOS (emios301).
/// Se usa ÚNICAMENTE para autenticación (solo lectura). Mapeo ajustado al esquema real:
/// id (varchar, es el login), contrasenya, nombre, perfil.
/// </summary>
public class Usuario
{
    /// <summary>Columna <c>id</c>: es el login del usuario (varchar, PK).</summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>Columna <c>contrasenya</c>: hash crypt(3).</summary>
    public string? PasswordHash { get; set; }

    /// <summary>Columna <c>nombre</c>.</summary>
    public string? Nombre { get; set; }

    /// <summary>Columna <c>perfil</c>: rol del usuario.</summary>
    public string? Perfil { get; set; }
}
