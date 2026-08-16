using MediatR;
using Inventario.Application.Dtos;
using Inventario.Application.Mappings;
using Inventario.Domain.Interfaces;

namespace Inventario.Application.Sistemas.Queries;

public record ObtenerSistemaQuery(int Id) : IRequest<SistemaDto>;

public class ObtenerSistemaQueryHandler : IRequestHandler<ObtenerSistemaQuery, SistemaDto>
{
    private readonly ISistemaRepository _sistemaRepository;
    private readonly ILocalizacionRepository _localizacionRepository;

    public ObtenerSistemaQueryHandler(
        ISistemaRepository sistemaRepository,
        ILocalizacionRepository localizacionRepository)
    {
        _sistemaRepository = sistemaRepository;
        _localizacionRepository = localizacionRepository;
    }

    public async Task<SistemaDto> Handle(ObtenerSistemaQuery request, CancellationToken ct)
    {
        var sistema = await _sistemaRepository.ObtenerPorIdAsync(request.Id, ct)
            ?? throw new Common.Exceptions.NotFoundException("Sistema", request.Id);

        var localizacion = await _localizacionRepository.ObtenerPorIdAsync(sistema.LocalizacionId, ct);
        return sistema.ToDto(localizacion?.Nombre ?? "—");
    }
}
