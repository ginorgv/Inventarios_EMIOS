using Inventario.Application.Dtos;
using Inventario.Domain.Entities;

namespace Inventario.Application.Mappings;

public static class DocumentoMappings
{
    public static DocumentoDto ToDto(this Documento d) => new(
        d.Id,
        d.Nombre,
        d.Descripcion,
        d.TipoDocumento,
        d.ContentType,
        d.TamanoBytes,
        d.EntidadTipo,
        d.EntidadId,
        d.UsuarioSubio,
        d.FechaSubida);
}
