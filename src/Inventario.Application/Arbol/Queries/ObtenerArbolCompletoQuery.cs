using MediatR;
using Inventario.Application.Dtos;
using Inventario.Application.Mappings;
using Inventario.Domain.Interfaces;

namespace Inventario.Application.Arbol.Queries;

public record ObtenerArbolCompletoQuery : IRequest<List<ArbolDto>>;

public class ObtenerArbolCompletoQueryHandler : IRequestHandler<ObtenerArbolCompletoQuery, List<ArbolDto>>
{
    private readonly IRedRepository _redRepository;
    private readonly ILocalizacionRepository _localizacionRepository;
    private readonly ISistemaRepository _sistemaRepository;
    private readonly IActivoRepository _activoRepository;
    private readonly IComponenteRepository _componenteRepository;

    public ObtenerArbolCompletoQueryHandler(
        IRedRepository redRepository,
        ILocalizacionRepository localizacionRepository,
        ISistemaRepository sistemaRepository,
        IActivoRepository activoRepository,
        IComponenteRepository componenteRepository)
    {
        _redRepository = redRepository;
        _localizacionRepository = localizacionRepository;
        _sistemaRepository = sistemaRepository;
        _activoRepository = activoRepository;
        _componenteRepository = componenteRepository;
    }

    public async Task<List<ArbolDto>> Handle(ObtenerArbolCompletoQuery request, CancellationToken ct)
    {
        // Datos de emios301 (solo lectura) + emios_inventario (escritura).
        var redes = await _redRepository.ObtenerTodasAsync(ct);
        var localizaciones = await _localizacionRepository.ObtenerTodasAsync(ct);
        var sistemas = await _sistemaRepository.ObtenerTodosAsync(ct);
        var activos = await _activoRepository.ObtenerTodosAsync(ct);

        var componentes = new List<Inventario.Domain.Entities.Componente>();
        foreach (var grupo in activos.GroupBy(a => a.Id))
            componentes.AddRange(await _componenteRepository.ObtenerPorActivoAsync(grupo.Key, ct));

        return ArbolMappings.Construir(redes, localizaciones, sistemas, activos, componentes);
    }
}
