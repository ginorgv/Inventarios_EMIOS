using MediatR;
using Inventario.Domain.Interfaces;

namespace Inventario.Application.Sistemas.Commands;

public record EliminarSistemaCommand(int Id) : IRequest;

public class EliminarSistemaCommandHandler : IRequestHandler<EliminarSistemaCommand>
{
    private readonly ISistemaRepository _sistemaRepository;
    private readonly IUnitOfWork _unitOfWork;

    public EliminarSistemaCommandHandler(ISistemaRepository sistemaRepository, IUnitOfWork unitOfWork)
    {
        _sistemaRepository = sistemaRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(EliminarSistemaCommand request, CancellationToken ct)
    {
        var sistema = await _sistemaRepository.ObtenerPorIdAsync(request.Id, ct)
            ?? throw new Common.Exceptions.NotFoundException("Sistema", request.Id);

        if (sistema.Activos.Count > 0)
            throw new InvalidOperationException(
                $"El sistema '{sistema.Nombre}' tiene {sistema.Activos.Count} activos asociados y no puede eliminarse.");

        _sistemaRepository.Eliminar(sistema);
        await _unitOfWork.SaveChangesAsync(ct);
    }
}
