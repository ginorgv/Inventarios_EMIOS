using MediatR;
using Inventario.Application.Dtos;
using Inventario.Application.Mappings;
using Inventario.Domain.Interfaces;

namespace Inventario.Application.Sistemas.Queries;

public record ListarSistemasQuery : IRequest<IReadOnlyList<SistemaDto>>;

public class ListarSistemasQueryHandler : IRequestHandler<ListarSistemasQuery, IReadOnlyList<SistemaDto>>
{
    private readonly ISistemaRepository _sistemaRepository;
    private readonly ILocalizacionRepository _localizacionRepository;

    public ListarSistemasQueryHandler(
        ISistemaRepository sistemaRepository,
        ILocalizacionRepository localizacionRepository)
    {
        _sistemaRepository = sistemaRepository;
        _localizacionRepository = localizacionRepository;
    }

    public async Task<IReadOnlyList<SistemaDto>> Handle(ListarSistemasQuery request, CancellationToken ct)
    {
        var sistemas = await _sistemaRepository.ObtenerTodosAsync(ct);
        var localizaciones = await _localizacionRepository.ObtenerTodasAsync(ct);
        var nombresLocalizacion = localizaciones.ToDictionary(l => l.Id, l => l.Nombre);

        return sistemas
            .Select(s => s.ToDto(nombresLocalizacion.TryGetValue(s.LocalizacionId, out var n) ? n : "—"))
            .ToList();
    }
}
