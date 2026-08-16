using MediatR;
using Inventario.Domain.Interfaces;

namespace Inventario.Application.Activos.Commands;

public record EliminarActivoCommand(int Id) : IRequest;

public class EliminarActivoCommandHandler : IRequestHandler<EliminarActivoCommand>
{
    private readonly IActivoRepository _activoRepository;
    private readonly IUnitOfWork _unitOfWork;

    public EliminarActivoCommandHandler(IActivoRepository activoRepository, IUnitOfWork unitOfWork)
    {
        _activoRepository = activoRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(EliminarActivoCommand request, CancellationToken ct)
    {
        var activo = await _activoRepository.ObtenerPorIdAsync(request.Id, incluirRelaciones: false, ct)
            ?? throw new Common.Exceptions.NotFoundException("Activo", request.Id);

        _activoRepository.Eliminar(activo);
        await _unitOfWork.SaveChangesAsync(ct);
    }
}
