using Inventario.Domain.ValueObjects;

namespace Inventario.Application.Dtos;

public record ActivoDto(
    int Id,
    string Codigo,
    string Nombre,
    string? Descripcion,
    int SistemaId,
    int LocalizacionId,
    string SistemaNombre,
    string? TipoActivo,
    EstadoActivo Estado,
    double? Latitud,
    double? Longitud,
    string? Fabricante,
    string? Modelo,
    string? NumeroSerie,
    DateTime? FechaInstalacion,
    int CantidadComponentes);
