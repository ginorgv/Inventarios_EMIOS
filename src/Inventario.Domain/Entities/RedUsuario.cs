namespace Inventario.Domain.Entities;

/// <summary>
/// Asignación usuario → red (tabla <c>redes_usuarios</c> de emios301, SOLO LECTURA).
/// Se usa únicamente en la lógica de filtrado multi-tenant: un usuario ve los
/// clientes/localizaciones de las redes a las que tiene acceso. La "red" NUNCA
/// se muestra en la interfaz.
/// </summary>
public class RedUsuario
{
    public string Usuario { get; set; } = string.Empty;

    public int RedId { get; set; }
}
