using MediatR;
using Inventario.Application.Dtos;
using Inventario.Application.Mappings;
using Inventario.Domain.Interfaces;

namespace Inventario.Application.Activos.Queries;

public record ListarComponentesQuery(int ActivoId) : IRequest<IReadOnlyList<ComponenteDto>>;

public class ListarComponentesQueryHandler
    : IRequestHandler<ListarComponentesQuery, IReadOnlyList<ComponenteDto>>
{
    private readonly IComponenteRepository _componenteRepository;

    public ListarComponentesQueryHandler(IComponenteRepository componenteRepository)
    {
        _componenteRepository = componenteRepository;
    }

    public async Task<IReadOnlyList<ComponenteDto>> Handle(ListarComponentesQuery request, CancellationToken ct)
    {
        var componentes = await _componenteRepository.ObtenerPorActivoAsync(request.ActivoId, ct);
        return componentes.Select(c => c.ToDto()).ToList();
    }
}
