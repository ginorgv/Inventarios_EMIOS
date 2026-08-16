using Inventario.Application.Dtos;
using Inventario.Domain.Entities;

namespace Inventario.Application.Mappings;

/// <summary>
/// Mapeo explícito de entidades de activos a DTOs (sin AutoMapper).
/// </summary>
public static class ActivoMappings
{
    public static ActivoDto ToDto(this Activo a, string sistemaNombre, int localizacionId) => new(
        a.Id,
        a.Codigo,
        a.Nombre,
        a.Descripcion,
        a.SistemaId,
        localizacionId,
        sistemaNombre,
        a.TipoActivo,
        a.Estado,
        a.Ubicacion?.Latitud,
        a.Ubicacion?.Longitud,
        a.Fabricante,
        a.Modelo,
        a.NumeroSerie,
        a.FechaInstalacion,
        a.Componentes.Count);

    public static ComponenteDto ToDto(this Componente c) => new(
        c.Id,
        c.Codigo,
        c.Nombre,
        c.Tipo,
        c.Descripcion,
        c.RangoMedicion?.Minimo,
        c.RangoMedicion?.Maximo,
        c.RangoMedicion?.Unidad,
        c.ActivoId,
        c.SensorId);
}
