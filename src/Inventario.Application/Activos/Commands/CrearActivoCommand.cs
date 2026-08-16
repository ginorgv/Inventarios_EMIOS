using MediatR;
using Inventario.Domain.Entities;
using Inventario.Domain.Interfaces;
using Inventario.Domain.ValueObjects;

namespace Inventario.Application.Activos.Commands;

public record CrearActivoCommand(
    string Codigo,
    string Nombre,
    string? Descripcion,
    int SistemaId,
    string? TipoActivo,
    EstadoActivo Estado,
    double? Latitud,
    double? Longitud,
    string? Fabricante,
    string? Modelo,
    string? NumeroSerie,
    DateTime? FechaInstalacion) : IRequest<int>;

public class CrearActivoCommandHandler : IRequestHandler<CrearActivoCommand, int>
{
    private readonly IActivoRepository _activoRepository;
    private readonly ISistemaRepository _sistemaRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CrearActivoCommandHandler(
        IActivoRepository activoRepository,
        ISistemaRepository sistemaRepository,
        IUnitOfWork unitOfWork)
    {
        _activoRepository = activoRepository;
        _sistemaRepository = sistemaRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<int> Handle(CrearActivoCommand request, CancellationToken ct)
    {
        if (await _sistemaRepository.ObtenerPorIdAsync(request.SistemaId, ct) is null)
            throw new Common.Exceptions.NotFoundException("Sistema", request.SistemaId);

        if (await _activoRepository.CodigoExisteAsync(request.Codigo, ct: ct))
            throw new InvalidOperationException($"Ya existe un activo con el código '{request.Codigo}'.");

        var activo = new Activo
        {
            Codigo = request.Codigo.Trim(),
            Nombre = request.Nombre.Trim(),
            Descripcion = request.Descripcion,
            SistemaId = request.SistemaId,
            TipoActivo = request.TipoActivo,
            Estado = request.Estado,
            Ubicacion = request.Latitud.HasValue && request.Longitud.HasValue
                ? new Coordenadas(request.Latitud.Value, request.Longitud.Value)
                : null,
            Fabricante = request.Fabricante,
            Modelo = request.Modelo,
            NumeroSerie = request.NumeroSerie,
            FechaInstalacion = request.FechaInstalacion
        };

        await _activoRepository.AgregarAsync(activo, ct);
        await _unitOfWork.SaveChangesAsync(ct);
        return activo.Id;
    }
}
