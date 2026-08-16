using MediatR;
using Inventario.Domain.Interfaces;

namespace Inventario.Application.Sistemas.Commands;

public record ActualizarSistemaCommand(
    int Id,
    string Codigo,
    string Nombre,
    string? Descripcion,
    int LocalizacionId,
    bool Activo) : IRequest;

public class ActualizarSistemaCommandHandler : IRequestHandler<ActualizarSistemaCommand>
{
    private readonly ISistemaRepository _sistemaRepository;
    private readonly ILocalizacionRepository _localizacionRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ActualizarSistemaCommandHandler(
        ISistemaRepository sistemaRepository,
        ILocalizacionRepository localizacionRepository,
        IUnitOfWork unitOfWork)
    {
        _sistemaRepository = sistemaRepository;
        _localizacionRepository = localizacionRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(ActualizarSistemaCommand request, CancellationToken ct)
    {
        var sistema = await _sistemaRepository.ObtenerPorIdAsync(request.Id, ct)
            ?? throw new Common.Exceptions.NotFoundException("Sistema", request.Id);

        if (await _localizacionRepository.ObtenerPorIdAsync(request.LocalizacionId, ct) is null)
            throw new Common.Exceptions.NotFoundException("Localizacion", request.LocalizacionId);

        if (await _sistemaRepository.CodigoExisteAsync(request.Codigo, request.Id, ct))
            throw new InvalidOperationException($"Ya existe un sistema con el código '{request.Codigo}'.");

        sistema.Codigo = request.Codigo.Trim();
        sistema.Nombre = request.Nombre.Trim();
        sistema.Descripcion = request.Descripcion;
        sistema.LocalizacionId = request.LocalizacionId;
        sistema.Activo = request.Activo;
        sistema.ModificadoEn = DateTime.UtcNow;

        _sistemaRepository.Actualizar(sistema);
        await _unitOfWork.SaveChangesAsync(ct);
    }
}
