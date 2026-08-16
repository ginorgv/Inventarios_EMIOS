using Inventario.Domain.Entities;
using Inventario.Domain.Interfaces;

namespace Inventario.Infrastructure.Services;

/// <summary>
/// Datos ampliados del Cliente (tabla <c>cliente_datos</c> en emios_inventario, 1:1
/// con emios301.clientes mediante Id = clientes.id). emios301 no se modifica.
/// </summary>
public class ServicioClienteDatos
{
    private readonly IClienteDatosRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public ServicioClienteDatos(IClienteDatosRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public Task<IReadOnlyList<ClienteDatos>> ObtenerTodosAsync(CancellationToken ct = default)
        => _repository.ObtenerTodosAsync(ct);

    public Task<ClienteDatos?> ObtenerPorClienteIdAsync(int clienteId, CancellationToken ct = default)
        => _repository.ObtenerPorClienteIdAsync(clienteId, ct);

    /// <summary>Inserta o actualiza (upsert por Id = id del cliente).</summary>
    public async Task GuardarAsync(ClienteDatos datos, CancellationToken ct = default)
    {
        var existente = await _repository.ObtenerPorClienteIdAsync(datos.Id, ct);
        if (existente is null)
        {
            await _repository.AgregarAsync(datos, ct);
        }
        else
        {
            existente.ContractRef = datos.ContractRef;
            existente.ProjectType = datos.ProjectType;
            existente.StartDate = datos.StartDate;
            existente.EndDate = datos.EndDate;
            _repository.Actualizar(existente);
        }

        await _unitOfWork.SaveChangesAsync(ct);
    }
}
