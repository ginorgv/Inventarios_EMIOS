using MediatR;
using Inventario.Application.Dtos;
using Inventario.Domain.Interfaces;

namespace Inventario.Application.Arbol.Queries;

public record ListarLocalizacionesQuery(int? RedId = null) : IRequest<IReadOnlyList<LocalizacionDto>>;

public class ListarLocalizacionesQueryHandler
    : IRequestHandler<ListarLocalizacionesQuery, IReadOnlyList<LocalizacionDto>>
{
    private readonly ILocalizacionRepository _localizacionRepository;
    private readonly IRedRepository _redRepository;

    public ListarLocalizacionesQueryHandler(
        ILocalizacionRepository localizacionRepository,
        IRedRepository redRepository)
    {
        _localizacionRepository = localizacionRepository;
        _redRepository = redRepository;
    }

    public async Task<IReadOnlyList<LocalizacionDto>> Handle(ListarLocalizacionesQuery request, CancellationToken ct)
    {
        var localizaciones = request.RedId.HasValue
            ? await _localizacionRepository.ObtenerPorRedAsync(request.RedId.Value, ct)
            : await _localizacionRepository.ObtenerTodasAsync(ct);

        var redes = await _redRepository.ObtenerTodasAsync(ct);
        var nombresRed = redes.ToDictionary(r => r.Id, r => r.Nombre);

        return localizaciones
            .Select(l => new LocalizacionDto(
                l.Id, l.Nombre, l.RedId,
                nombresRed.TryGetValue(l.RedId, out var n) ? n : "—"))
            .ToList();
    }
}
