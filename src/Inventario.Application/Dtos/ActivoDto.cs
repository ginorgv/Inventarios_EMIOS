using Inventario.Domain.ValueObjects;

namespace Inventario.Application.Dtos;

public record ActivoDto(
    int Id,
    string Codigo,
    string Nombre,
    int SistemaId,
    int LocalizacionId,
    string SistemaNombre,
    EstadoActivo Estado,
    string? Fabricante,
    string? Modelo,
    string? NumeroSerie,
    DateTime? FechaInstalacion,
    decimal? PotenciaNominalKw,
    decimal? EficienciaPct,
    DateTime? FinGarantia,
    DateTime? UltimaRevision,
    DateTime? ProximaRevision,
    int CantidadComponentes);
