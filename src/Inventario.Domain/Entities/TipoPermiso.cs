namespace Inventario.Domain.Entities;

/// <summary>
/// Tipo de permiso sobre un nodo de la jerarquía. Se hereda hacia abajo:
///   Lectura        → ver el subárbol del nodo
///   Edicion        → + crear/editar (y mover sensores)
///   Administracion → + eliminar y gestionar permisos
/// </summary>
public enum TipoPermiso
{
    Ninguno = 0,
    Lectura = 1,
    Edicion = 2,
    Administracion = 3
}
