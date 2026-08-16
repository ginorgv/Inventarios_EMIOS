namespace Inventario.Application.Dtos;

public record MovimientoDto(
    int Id,
    int ActivoId,
    string ActivoCodigo,
    string ActivoNombre,
    string Tipo,
    DateTime Fecha,
    string? Origen,
    string? Destino,
    string? Usuario,
    string? Observaciones);
