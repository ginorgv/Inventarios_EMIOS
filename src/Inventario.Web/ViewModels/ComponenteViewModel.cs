using Inventario.Application.Activos.Commands;
using Inventario.Application.Dtos;

namespace Inventario.Web.ViewModels;

/// <summary>Modelo de formulario para crear/editar un Componente/Sensor (Nivel 5).</summary>
public class ComponenteViewModel
{
    public int Id { get; set; }
    public int ActivoId { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string? Tipo { get; set; }
    public string? Descripcion { get; set; }
    public decimal? RangoMinimo { get; set; }
    public decimal? RangoMaximo { get; set; }
    public string? RangoUnidad { get; set; }

    public static ComponenteViewModel FromDto(ComponenteDto dto) => new()
    {
        Id = dto.Id,
        ActivoId = dto.ActivoId,
        Codigo = dto.Codigo,
        Nombre = dto.Nombre,
        Tipo = dto.Tipo,
        Descripcion = dto.Descripcion,
        RangoMinimo = dto.RangoMinimo,
        RangoMaximo = dto.RangoMaximo,
        RangoUnidad = dto.RangoUnidad
    };

    public CrearComponenteCommand ToCrearCommand() => new(
        ActivoId, Codigo, Nombre, Tipo, Descripcion, RangoMinimo, RangoMaximo, RangoUnidad);

    public ActualizarComponenteCommand ToActualizarCommand() => new(
        Id, Codigo, Nombre, Tipo, Descripcion, RangoMinimo, RangoMaximo, RangoUnidad);
}
