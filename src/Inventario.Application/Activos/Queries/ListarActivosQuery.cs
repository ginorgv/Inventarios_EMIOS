using MediatR;
using Inventario.Application.Dtos;
using Inventario.Application.Mappings;
using Inventario.Domain.Interfaces;

namespace Inventario.Application.Activos.Queries;

public record ListarActivosQuery : IRequest<IReadOnlyList<ActivoDto>>;

public class ListarActivosQueryHandler : IRequestHandler<ListarActivosQuery, IReadOnlyList<ActivoDto>>
{
    private readonly IActivoRepository _activoRepository;
    private readonly ISistemaRepository _sistemaRepository;

    public ListarActivosQueryHandler(IActivoRepository activoRepository, ISistemaRepository sistemaRepository)
    {
        _activoRepository = activoRepository;
        _sistemaRepository = sistemaRepository;
    }

    public async Task<IReadOnlyList<ActivoDto>> Handle(ListarActivosQuery request, CancellationToken ct)
    {
        var activos = await _activoRepository.ObtenerTodosAsync(ct);
        var sistemas = await _sistemaRepository.ObtenerTodosAsync(ct);
        var nombresSistema = sistemas.ToDictionary(s => s.Id, s => s.Nombre);
        var localizacionPorSistema = sistemas.ToDictionary(s => s.Id, s => s.LocalizacionId);

        return activos
            .Select(a => a.ToDto(
                nombresSistema.TryGetValue(a.SistemaId, out var n) ? n : "—",
                localizacionPorSistema.TryGetValue(a.SistemaId, out var loc) ? loc : 0))
            .ToList();
    }
}
