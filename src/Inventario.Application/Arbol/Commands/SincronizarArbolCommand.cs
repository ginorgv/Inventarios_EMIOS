using MediatR;
using Inventario.Domain.Interfaces;

namespace Inventario.Application.Arbol.Commands;

/// <summary>
/// Sincroniza el árbol entre la BD heredada (emios301) y la nueva (emios_inventario).
/// En v1.00 los niveles 1-2 son de solo lectura, así que la sincronización se limita a
/// registrar la operación. En fases posteriores podrá propagar datos de catálogo.
/// </summary>
public record SincronizarArbolCommand : IRequest;

public class SincronizarArbolCommandHandler : IRequestHandler<SincronizarArbolCommand>
{
    private readonly IRedRepository _redRepository;
    private readonly ILocalizacionRepository _localizacionRepository;

    public SincronizarArbolCommandHandler(
        IRedRepository redRepository,
        ILocalizacionRepository localizacionRepository)
    {
        _redRepository = redRepository;
        _localizacionRepository = localizacionRepository;
    }

    public async Task Handle(SincronizarArbolCommand request, CancellationToken ct)
    {
        // Validación de accesibilidad a la BD heredada (solo lectura).
        var redes = await _redRepository.ObtenerTodasAsync(ct);
        var localizaciones = await _localizacionRepository.ObtenerTodasAsync(ct);

        // TODO v1.1: persistir catálogo sincronizado / marcar fechas de última sincronización.
        await Task.CompletedTask;
    }
}
