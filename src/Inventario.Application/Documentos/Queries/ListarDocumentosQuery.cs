using MediatR;
using Inventario.Application.Dtos;
using Inventario.Application.Mappings;
using Inventario.Domain.Interfaces;

namespace Inventario.Application.Documentos.Queries;

public record ListarDocumentosQuery(string EntidadTipo, int EntidadId) : IRequest<IReadOnlyList<DocumentoDto>>;

public class ListarDocumentosQueryHandler : IRequestHandler<ListarDocumentosQuery, IReadOnlyList<DocumentoDto>>
{
    private readonly IDocumentoRepository _documentoRepository;

    public ListarDocumentosQueryHandler(IDocumentoRepository documentoRepository)
    {
        _documentoRepository = documentoRepository;
    }

    public async Task<IReadOnlyList<DocumentoDto>> Handle(ListarDocumentosQuery request, CancellationToken ct)
    {
        var documentos = await _documentoRepository.ObtenerPorEntidadAsync(request.EntidadTipo, request.EntidadId, ct);
        return documentos.Select(d => d.ToDto()).ToList();
    }
}
