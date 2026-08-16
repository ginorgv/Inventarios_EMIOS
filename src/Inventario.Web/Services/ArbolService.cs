using Inventario.Application.Arbol.Commands;
using Inventario.Application.Arbol.Queries;
using Inventario.Application.Dtos;
using MediatR;

namespace Inventario.Web.Services;

public interface IArbolService
{
    Task<List<ArbolDto>> ObtenerArbolAsync(CancellationToken ct = default);
    Task SincronizarAsync(CancellationToken ct = default);
}

public class ArbolService : IArbolService
{
    private readonly ISender _sender;

    public ArbolService(ISender sender)
    {
        _sender = sender;
    }

    public Task<List<ArbolDto>> ObtenerArbolAsync(CancellationToken ct = default)
        => _sender.Send(new ObtenerArbolCompletoQuery(), ct);

    public Task SincronizarAsync(CancellationToken ct = default)
        => _sender.Send(new SincronizarArbolCommand(), ct);
}
