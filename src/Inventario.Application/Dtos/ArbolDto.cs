namespace Inventario.Application.Dtos;

/// <summary>
/// Nodo genérico del árbol jerárquico:
/// Red → Localización → Sistema → Activo → Componente.
/// </summary>
public class ArbolDto
{
    public int Nivel { get; set; }          // 1..5
    public string Tipo { get; set; } = string.Empty;  // Red | Localizacion | Sistema | Activo | Componente
    public int Id { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string? Estado { get; set; }
    public bool TieneHijos => Hijos.Count > 0;
    public List<ArbolDto> Hijos { get; set; } = new();
}
