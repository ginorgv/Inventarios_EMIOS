using MediatR;
using Inventario.Domain.Interfaces;
using Inventario.Domain.ValueObjects;

namespace Inventario.Application.Activos.Commands;

public record ActualizarComponenteCommand(
    int Id,
    string Codigo,
    string Nombre,
    string? Tipo,
    string? Descripcion,
    decimal? RangoMinimo,
    decimal? RangoMaximo,
    string? RangoUnidad) : IRequest;

public class ActualizarComponenteCommandHandler : IRequestHandler<ActualizarComponenteCommand>
{
    private readonly IComponenteRepository _componenteRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ActualizarComponenteCommandHandler(
        IComponenteRepository componenteRepository,
        IUnitOfWork unitOfWork)
    {
        _componenteRepository = componenteRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(ActualizarComponenteCommand request, CancellationToken ct)
    {
        var componente = await _componenteRepository.ObtenerPorIdAsync(request.Id, ct)
            ?? throw new Common.Exceptions.NotFoundException("Componente", request.Id);

        componente.Codigo = request.Codigo.Trim();
        componente.Nombre = request.Nombre.Trim();
        componente.Tipo = request.Tipo;
        componente.Descripcion = request.Descripcion;
        componente.RangoMedicion = request.RangoMinimo.HasValue && request.RangoMaximo.HasValue
            ? new RangoMedicion(request.RangoMinimo.Value, request.RangoMaximo.Value, request.RangoUnidad ?? "")
            : null;
        componente.ModificadoEn = DateTime.UtcNow;

        _componenteRepository.Actualizar(componente);
        await _unitOfWork.SaveChangesAsync(ct);
    }
}
