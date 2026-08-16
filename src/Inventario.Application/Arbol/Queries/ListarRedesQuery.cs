using MediatR;
using Inventario.Application.Dtos;
using Inventario.Domain.Interfaces;

namespace Inventario.Application.Arbol.Queries;

public record ListarRedesQuery : IRequest<IReadOnlyList<RedDto>>;

public class ListarRedesQueryHandler : IRequestHandler<ListarRedesQuery, IReadOnlyList<RedDto>>
{
    private readonly IRedRepository _redRepository;

    public ListarRedesQueryHandler(IRedRepository redRepository)
    {
        _redRepository = redRepository;
    }

    public async Task<IReadOnlyList<RedDto>> Handle(ListarRedesQuery request, CancellationToken ct)
    {
        var redes = await _redRepository.ObtenerTodasAsync(ct);
        return redes.Select(r => new RedDto(r.Id, r.Nombre)).ToList();
    }
}
