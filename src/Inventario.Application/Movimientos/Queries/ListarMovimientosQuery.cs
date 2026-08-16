using MediatR;
using Inventario.Application.Dtos;
using Inventario.Domain.Entities;
using Inventario.Domain.Interfaces;

namespace Inventario.Application.Movimientos.Queries;

public record ListarMovimientosQuery : IRequest<IReadOnlyList<MovimientoDto>>;

public class ListarMovimientosQueryHandler : IRequestHandler<ListarMovimientosQuery, IReadOnlyList<MovimientoDto>>
{
    private readonly IMovimientoRepository _movimientoRepository;

    public ListarMovimientosQueryHandler(IMovimientoRepository movimientoRepository)
    {
        _movimientoRepository = movimientoRepository;
    }

    public async Task<IReadOnlyList<MovimientoDto>> Handle(ListarMovimientosQuery request, CancellationToken ct)
    {
        var movimientos = await _movimientoRepository.ObtenerTodosAsync(ct);
        return movimientos.Select(ToDto).ToList();
    }

    internal static MovimientoDto ToDto(Movimiento m) => new(
        m.Id,
        m.ActivoId,
        m.Activo?.Codigo ?? "—",
        m.Activo?.Nombre ?? "—",
        m.Tipo,
        m.Fecha,
        m.Origen,
        m.Destino,
        m.Usuario,
        m.Observaciones);
}
