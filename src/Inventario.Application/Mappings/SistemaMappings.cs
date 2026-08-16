using Inventario.Application.Dtos;
using Inventario.Domain.Entities;

namespace Inventario.Application.Mappings;

public static class SistemaMappings
{
    public static SistemaDto ToDto(this Sistema s, string localizacionNombre) => new(
        s.Id,
        s.Codigo,
        s.Nombre,
        s.Descripcion,
        s.LocalizacionId,
        localizacionNombre,
        s.Activo,
        s.Activos.Count);
}
