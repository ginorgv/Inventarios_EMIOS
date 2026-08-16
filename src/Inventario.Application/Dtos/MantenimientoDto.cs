namespace Inventario.Application.Dtos;

public record MantenimientoDto(
    int Id,
    int ActivoId,
    string ActivoCodigo,
    string ActivoNombre,
    string Tipo,
    DateTime? FechaProgramada,
    DateTime? FechaEjecucion,
    string? Descripcion,
    decimal? Costo,
    string? Responsable,
    string Estado);
