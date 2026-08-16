namespace Inventario.Application.Dtos;

public record DocumentoDto(
    int Id,
    string Nombre,
    string? Descripcion,
    string TipoDocumento,
    string ContentType,
    long TamanoBytes,
    string EntidadTipo,
    int EntidadId,
    string? UsuarioSubio,
    DateTime FechaSubida);
