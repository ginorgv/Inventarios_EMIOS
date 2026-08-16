namespace Inventario.Application.Dtos;

public record UsuarioInventarioDto(
    int Id,
    string NombreUsuario,
    string NombreCompleto,
    string? Email,
    string Rol,
    bool Activo);
