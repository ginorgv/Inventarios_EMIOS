using MediatR;
using Inventario.Application.Dtos;
using Inventario.Domain.Entities;
using Inventario.Domain.Interfaces;

namespace Inventario.Application.Mantenimientos.Queries;

public record ListarMantenimientosQuery : IRequest<IReadOnlyList<MantenimientoDto>>;

public class ListarMantenimientosQueryHandler
    : IRequestHandler<ListarMantenimientosQuery, IReadOnlyList<MantenimientoDto>>
{
    private readonly IMantenimientoRepository _mantenimientoRepository;

    public ListarMantenimientosQueryHandler(IMantenimientoRepository mantenimientoRepository)
    {
        _mantenimientoRepository = mantenimientoRepository;
    }

    public async Task<IReadOnlyList<MantenimientoDto>> Handle(ListarMantenimientosQuery request, CancellationToken ct)
    {
        var mantenimientos = await _mantenimientoRepository.ObtenerTodosAsync(ct);
        return mantenimientos.Select(ToDto).ToList();
    }

    internal static MantenimientoDto ToDto(Mantenimiento m) => new(
        m.Id,
        m.ActivoId,
        m.Activo?.Codigo ?? "—",
        m.Activo?.Nombre ?? "—",
        m.Tipo,
        m.FechaProgramada,
        m.FechaEjecucion,
        m.Descripcion,
        m.Costo,
        m.Responsable,
        m.Estado);
}
