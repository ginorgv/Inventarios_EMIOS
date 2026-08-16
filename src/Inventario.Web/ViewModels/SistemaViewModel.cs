using Inventario.Application.Dtos;
using Inventario.Application.Sistemas.Commands;

namespace Inventario.Web.ViewModels;

/// <summary>Modelo de formulario para crear/editar un Sistema (Nivel 3).</summary>
public class SistemaViewModel
{
    public int Id { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public int LocalizacionId { get; set; }
    public bool Activo { get; set; } = true;

    public static SistemaViewModel FromDto(SistemaDto dto) => new()
    {
        Id = dto.Id,
        Codigo = dto.Codigo,
        Nombre = dto.Nombre,
        Descripcion = dto.Descripcion,
        LocalizacionId = dto.LocalizacionId,
        Activo = dto.Activo
    };

    public CrearSistemaCommand ToCrearCommand() => new(Codigo, Nombre, Descripcion, LocalizacionId, Activo);

    public ActualizarSistemaCommand ToActualizarCommand() => new(Id, Codigo, Nombre, Descripcion, LocalizacionId, Activo);
}
