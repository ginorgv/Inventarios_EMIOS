using MediatR;
using Inventario.Domain.Entities;
using Inventario.Domain.Interfaces;
using Inventario.Domain.ValueObjects;

namespace Inventario.Application.Activos.Commands;

public record CrearComponenteCommand(
    int ActivoId,
    string Codigo,
    string Nombre,
    string? Tipo,
    string? Descripcion,
    decimal? RangoMinimo,
    decimal? RangoMaximo,
    string? RangoUnidad) : IRequest<int>;

public class CrearComponenteCommandHandler : IRequestHandler<CrearComponenteCommand, int>
{
    private readonly IComponenteRepository _componenteRepository;
    private readonly IActivoRepository _activoRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CrearComponenteCommandHandler(
        IComponenteRepository componenteRepository,
        IActivoRepository activoRepository,
        IUnitOfWork unitOfWork)
    {
        _componenteRepository = componenteRepository;
        _activoRepository = activoRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<int> Handle(CrearComponenteCommand request, CancellationToken ct)
    {
        if (await _activoRepository.ObtenerPorIdAsync(request.ActivoId, incluirRelaciones: false, ct) is null)
            throw new Common.Exceptions.NotFoundException("Activo", request.ActivoId);

        var componente = new Componente
        {
            ActivoId = request.ActivoId,
            Codigo = request.Codigo.Trim(),
            Nombre = request.Nombre.Trim(),
            Tipo = request.Tipo,
            Descripcion = request.Descripcion,
            RangoMedicion = request.RangoMinimo.HasValue && request.RangoMaximo.HasValue
                ? new RangoMedicion(request.RangoMinimo.Value, request.RangoMaximo.Value, request.RangoUnidad ?? "")
                : null
        };

        await _componenteRepository.AgregarAsync(componente, ct);
        await _unitOfWork.SaveChangesAsync(ct);
        return componente.Id;
    }
}
