using MediatR;
using Inventario.Domain.Entities;
using Inventario.Domain.Interfaces;

namespace Inventario.Application.Documentos.Commands;

public record SubirDocumentoCommand(
    string Nombre,
    string? Descripcion,
    string TipoDocumento,
    string ContentType,
    long TamanoBytes,
    string EntidadTipo,
    int EntidadId,
    string RutaAlmacenamiento,
    string? Usuario) : IRequest<int>;

public class SubirDocumentoCommandHandler : IRequestHandler<SubirDocumentoCommand, int>
{
    private readonly IDocumentoRepository _documentoRepository;
    private readonly IUnitOfWork _unitOfWork;

    public SubirDocumentoCommandHandler(IDocumentoRepository documentoRepository, IUnitOfWork unitOfWork)
    {
        _documentoRepository = documentoRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<int> Handle(SubirDocumentoCommand request, CancellationToken ct)
    {
        var documento = new Documento
        {
            Nombre = request.Nombre,
            Descripcion = request.Descripcion,
            TipoDocumento = request.TipoDocumento,
            ContentType = request.ContentType,
            TamanoBytes = request.TamanoBytes,
            EntidadTipo = request.EntidadTipo,
            EntidadId = request.EntidadId,
            ActivoId = request.EntidadTipo == "Activo" ? request.EntidadId : null,
            UsuarioSubio = request.Usuario,
            FechaSubida = DateTime.UtcNow,
            // La ruta física la asigna el ServicioAlmacenamiento antes de llamar a este comando.
            RutaAlmacenamiento = request.RutaAlmacenamiento
        };

        await _documentoRepository.AgregarAsync(documento, ct);
        await _unitOfWork.SaveChangesAsync(ct);
        return documento.Id;
    }
}
