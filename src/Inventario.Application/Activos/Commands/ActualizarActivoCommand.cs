using MediatR;
using Inventario.Domain.Interfaces;
using Inventario.Domain.ValueObjects;

namespace Inventario.Application.Activos.Commands;

public record ActualizarActivoCommand(
    int Id,
    string Codigo,
    string Nombre,
    int SistemaId,
    EstadoActivo Estado,
    string? Fabricante,
    string? Modelo,
    string? NumeroSerie,
    DateTime? FechaInstalacion,
    decimal? PotenciaNominalKw,
    decimal? EficienciaPct,
    DateTime? FinGarantia,
    DateTime? UltimaRevision,
    DateTime? ProximaRevision) : IRequest;

public class ActualizarActivoCommandHandler : IRequestHandler<ActualizarActivoCommand>
{
    private readonly IActivoRepository _activoRepository;
    private readonly ISistemaRepository _sistemaRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ActualizarActivoCommandHandler(
        IActivoRepository activoRepository,
        ISistemaRepository sistemaRepository,
        IUnitOfWork unitOfWork)
    {
        _activoRepository = activoRepository;
        _sistemaRepository = sistemaRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(ActualizarActivoCommand request, CancellationToken ct)
    {
        var activo = await _activoRepository.ObtenerPorIdAsync(request.Id, incluirRelaciones: false, ct)
            ?? throw new Common.Exceptions.NotFoundException("Activo", request.Id);

        if (await _sistemaRepository.ObtenerPorIdAsync(request.SistemaId, ct) is null)
            throw new Common.Exceptions.NotFoundException("Sistema", request.SistemaId);

        if (await _activoRepository.CodigoExisteAsync(request.Codigo, request.Id, ct))
            throw new InvalidOperationException($"Ya existe un activo con el código '{request.Codigo}'.");

        activo.Codigo = request.Codigo.Trim();
        activo.Nombre = request.Nombre.Trim();
        activo.SistemaId = request.SistemaId;
        activo.Estado = request.Estado;
        activo.Fabricante = request.Fabricante;
        activo.Modelo = request.Modelo;
        activo.NumeroSerie = request.NumeroSerie;
        activo.FechaInstalacion = request.FechaInstalacion;
        activo.PotenciaNominalKw = request.PotenciaNominalKw;
        activo.EficienciaPct = request.EficienciaPct;
        activo.FinGarantia = request.FinGarantia;
        activo.UltimaRevision = request.UltimaRevision;
        activo.ProximaRevision = request.ProximaRevision;
        activo.ModificadoEn = DateTime.UtcNow;

        _activoRepository.Actualizar(activo);
        await _unitOfWork.SaveChangesAsync(ct);
    }
}
