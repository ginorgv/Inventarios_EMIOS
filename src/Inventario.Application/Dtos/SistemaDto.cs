namespace Inventario.Application.Dtos;

public record SistemaDto(
    int Id,
    string Codigo,
    string Nombre,
    string? Descripcion,
    int LocalizacionId,
    string LocalizacionNombre,
    bool Activo,
    int CantidadActivos);
