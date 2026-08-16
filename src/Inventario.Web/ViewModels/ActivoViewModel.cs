using Inventario.Application.Activos.Commands;
using Inventario.Application.Dtos;
using Inventario.Domain.ValueObjects;

namespace Inventario.Web.ViewModels;

/// <summary>Modelo de formulario para crear/editar un Activo (Nivel 4).</summary>
public class ActivoViewModel
{
    public int Id { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public int SistemaId { get; set; }
    public string? TipoActivo { get; set; }
    public EstadoActivo Estado { get; set; } = EstadoActivo.Activo;
    public double? Latitud { get; set; }
    public double? Longitud { get; set; }
    public string? Fabricante { get; set; }
    public string? Modelo { get; set; }
    public string? NumeroSerie { get; set; }
    public DateTime? FechaInstalacion { get; set; }

    public static ActivoViewModel FromDto(ActivoDto dto) => new()
    {
        Id = dto.Id,
        Codigo = dto.Codigo,
        Nombre = dto.Nombre,
        Descripcion = dto.Descripcion,
        SistemaId = dto.SistemaId,
        TipoActivo = dto.TipoActivo,
        Estado = dto.Estado,
        Latitud = dto.Latitud,
        Longitud = dto.Longitud,
        Fabricante = dto.Fabricante,
        Modelo = dto.Modelo,
        NumeroSerie = dto.NumeroSerie,
        FechaInstalacion = dto.FechaInstalacion
    };

    public CrearActivoCommand ToCrearCommand() => new(
        Codigo, Nombre, Descripcion, SistemaId, TipoActivo, Estado,
        Latitud, Longitud, Fabricante, Modelo, NumeroSerie, FechaInstalacion);

    public ActualizarActivoCommand ToActualizarCommand() => new(
        Id, Codigo, Nombre, Descripcion, SistemaId, TipoActivo, Estado,
        Latitud, Longitud, Fabricante, Modelo, NumeroSerie, FechaInstalacion);
}
