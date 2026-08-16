namespace Inventario.Application.Dtos;

public record ComponenteDto(
    int Id,
    string Codigo,
    string Nombre,
    string? Tipo,
    string? Descripcion,
    decimal? RangoMinimo,
    decimal? RangoMaximo,
    string? RangoUnidad,
    int ActivoId,
    int SensorId);
