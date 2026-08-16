using MediatR;
using Inventario.Application.Dtos;
using Inventario.Application.Mappings;
using Inventario.Domain.Interfaces;

namespace Inventario.Application.Activos.Queries;

public record ObtenerActivoQuery(int Id) : IRequest<ActivoDto>;

public class ObtenerActivoQueryHandler : IRequestHandler<ObtenerActivoQuery, ActivoDto>
{
    private readonly IActivoRepository _activoRepository;
    private readonly ISistemaRepository _sistemaRepository;

    public ObtenerActivoQueryHandler(IActivoRepository activoRepository, ISistemaRepository sistemaRepository)
    {
        _activoRepository = activoRepository;
        _sistemaRepository = sistemaRepository;
    }

    public async Task<ActivoDto> Handle(ObtenerActivoQuery request, CancellationToken ct)
    {
        var activo = await _activoRepository.ObtenerPorIdAsync(request.Id, incluirRelaciones: true, ct)
            ?? throw new Common.Exceptions.NotFoundException("Activo", request.Id);

        var sistema = await _sistemaRepository.ObtenerPorIdAsync(activo.SistemaId, ct);
        return activo.ToDto(sistema?.Nombre ?? "—", sistema?.LocalizacionId ?? 0);
    }
}
