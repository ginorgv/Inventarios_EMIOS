using MediatR;
using Inventario.Domain.Entities;
using Inventario.Domain.Interfaces;

namespace Inventario.Application.Sistemas.Commands;

public record CrearSistemaCommand(
    string Codigo,
    string Nombre,
    string? Descripcion,
    int LocalizacionId,
    bool Activo) : IRequest<int>;

public class CrearSistemaCommandHandler : IRequestHandler<CrearSistemaCommand, int>
{
    private readonly ISistemaRepository _sistemaRepository;
    private readonly ILocalizacionRepository _localizacionRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CrearSistemaCommandHandler(
        ISistemaRepository sistemaRepository,
        ILocalizacionRepository localizacionRepository,
        IUnitOfWork unitOfWork)
    {
        _sistemaRepository = sistemaRepository;
        _localizacionRepository = localizacionRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<int> Handle(CrearSistemaCommand request, CancellationToken ct)
    {
        // La localización vive en emios301 (solo lectura); validamos que exista.
        if (await _localizacionRepository.ObtenerPorIdAsync(request.LocalizacionId, ct) is null)
            throw new Common.Exceptions.NotFoundException("Localizacion", request.LocalizacionId);

        if (await _sistemaRepository.CodigoExisteAsync(request.Codigo, ct: ct))
            throw new InvalidOperationException($"Ya existe un sistema con el código '{request.Codigo}'.");

        var sistema = new Sistema
        {
            Codigo = request.Codigo.Trim(),
            Nombre = request.Nombre.Trim(),
            Descripcion = request.Descripcion,
            LocalizacionId = request.LocalizacionId,
            Activo = request.Activo
        };

        await _sistemaRepository.AgregarAsync(sistema, ct);
        await _unitOfWork.SaveChangesAsync(ct);
        return sistema.Id;
    }
}
