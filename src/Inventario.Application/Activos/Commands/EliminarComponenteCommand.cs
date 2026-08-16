using MediatR;
using Inventario.Domain.Interfaces;

namespace Inventario.Application.Activos.Commands;

public record EliminarComponenteCommand(int Id) : IRequest;

public class EliminarComponenteCommandHandler : IRequestHandler<EliminarComponenteCommand>
{
    private readonly IComponenteRepository _componenteRepository;
    private readonly IUnitOfWork _unitOfWork;

    public EliminarComponenteCommandHandler(
        IComponenteRepository componenteRepository,
        IUnitOfWork unitOfWork)
    {
        _componenteRepository = componenteRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(EliminarComponenteCommand request, CancellationToken ct)
    {
        var componente = await _componenteRepository.ObtenerPorIdAsync(request.Id, ct)
            ?? throw new Common.Exceptions.NotFoundException("Componente", request.Id);

        _componenteRepository.Eliminar(componente);
        await _unitOfWork.SaveChangesAsync(ct);
    }
}
