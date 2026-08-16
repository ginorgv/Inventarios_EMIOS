namespace Inventario.Domain.Entities;

/// <summary>
/// Cliente/Proyecto (Nivel 1 de la jerarquía).
/// Vista de SOLO LECTURA de la tabla <c>clientes</c> de emios301.
/// Se filtra por el usuario logueado (usuario → redes_usuarios → redes → cliente).
/// </summary>
public class Cliente
{
    public int Id { get; set; }

    public string Nombre { get; set; } = string.Empty;
}
