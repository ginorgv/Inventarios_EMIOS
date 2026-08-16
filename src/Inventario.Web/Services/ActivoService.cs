using Inventario.Application.Activos.Commands;
using Inventario.Application.Activos.Queries;
using Inventario.Application.Arbol.Queries;
using Inventario.Application.Documentos.Commands;
using Inventario.Application.Documentos.Queries;
using Inventario.Application.Dtos;
using Inventario.Application.Mantenimientos.Queries;
using Inventario.Application.Movimientos.Queries;
using Inventario.Application.Sistemas.Commands;
using Inventario.Application.Sistemas.Queries;
using Inventario.Application.Usuarios.Queries;
using Inventario.Infrastructure.Services;
using Inventario.Web.ViewModels;
using MediatR;

namespace Inventario.Web.Services;

public interface IActivoService
{
    Task<IReadOnlyList<ActivoDto>> ListarActivosAsync(CancellationToken ct = default);
    Task<ActivoDto> ObtenerActivoAsync(int id, CancellationToken ct = default);
    Task<int> CrearActivoAsync(ActivoViewModel modelo, CancellationToken ct = default);
    Task ActualizarActivoAsync(ActivoViewModel modelo, CancellationToken ct = default);
    Task EliminarActivoAsync(int id, CancellationToken ct = default);

    Task<IReadOnlyList<SistemaDto>> ListarSistemasAsync(CancellationToken ct = default);
    Task<int> CrearSistemaAsync(SistemaViewModel modelo, CancellationToken ct = default);
    Task ActualizarSistemaAsync(SistemaViewModel modelo, CancellationToken ct = default);
    Task EliminarSistemaAsync(int id, CancellationToken ct = default);

    Task<IReadOnlyList<ComponenteDto>> ListarComponentesAsync(int activoId, CancellationToken ct = default);
    Task<int> CrearComponenteAsync(ComponenteViewModel modelo, CancellationToken ct = default);
    Task EliminarComponenteAsync(int id, CancellationToken ct = default);

    Task<IReadOnlyList<LocalizacionDto>> ListarLocalizacionesAsync(int? redId = null, CancellationToken ct = default);
    Task<IReadOnlyList<RedDto>> ListarRedesAsync(CancellationToken ct = default);

    Task<IReadOnlyList<MovimientoDto>> ListarMovimientosAsync(CancellationToken ct = default);
    Task<IReadOnlyList<MantenimientoDto>> ListarMantenimientosAsync(CancellationToken ct = default);
    Task<IReadOnlyList<UsuarioInventarioDto>> ListarUsuariosAsync(CancellationToken ct = default);

    Task<IReadOnlyList<DocumentoDto>> ListarDocumentosAsync(string entidadTipo, int entidadId, CancellationToken ct = default);
    Task<int> SubirDocumentoAsync(string nombre, string? descripcion, string tipoDocumento, string contentType,
        long tamanoBytes, string entidadTipo, int entidadId, string rutaAlmacenamiento, string? usuario,
        CancellationToken ct = default);

    /// <summary>Importa los sensores de emios301 como Activos en emios_inventario.</summary>
    Task<ResultadoImportacion> ImportarSensoresAsync(CancellationToken ct = default);

    /// <summary>Sensores de una localización aún no asignados a ningún activo.</summary>
    Task<IReadOnlyList<Inventario.Domain.Entities.Sensor>> ListarSensoresDisponiblesAsync(
        int localizacionId, CancellationToken ct = default);

    /// <summary>Añade los sensores elegidos como Componentes al activo indicado.</summary>
    Task<int> ImportarSensoresAActivoAsync(int activoId, IEnumerable<int> sensorIds, CancellationToken ct = default);

    /// <summary>Mueve un componente/sensor de un activo a otro.</summary>
    Task MoverComponenteAsync(int componenteId, int nuevoActivoId, CancellationToken ct = default);
}

public class ActivoService : IActivoService
{
    private readonly ISender _sender;
    private readonly ServicioImportacionSensores _importacion;

    public ActivoService(ISender sender, ServicioImportacionSensores importacion)
    {
        _sender = sender;
        _importacion = importacion;
    }

    public Task<IReadOnlyList<ActivoDto>> ListarActivosAsync(CancellationToken ct = default)
        => _sender.Send(new ListarActivosQuery(), ct);

    public Task<ActivoDto> ObtenerActivoAsync(int id, CancellationToken ct = default)
        => _sender.Send(new ObtenerActivoQuery(id), ct);

    public Task<int> CrearActivoAsync(ActivoViewModel modelo, CancellationToken ct = default)
        => _sender.Send(modelo.ToCrearCommand(), ct);

    public Task ActualizarActivoAsync(ActivoViewModel modelo, CancellationToken ct = default)
        => _sender.Send(modelo.ToActualizarCommand(), ct);

    public Task EliminarActivoAsync(int id, CancellationToken ct = default)
        => _sender.Send(new EliminarActivoCommand(id), ct);

    public Task<IReadOnlyList<SistemaDto>> ListarSistemasAsync(CancellationToken ct = default)
        => _sender.Send(new ListarSistemasQuery(), ct);

    public Task<int> CrearSistemaAsync(SistemaViewModel modelo, CancellationToken ct = default)
        => _sender.Send(modelo.ToCrearCommand(), ct);

    public Task ActualizarSistemaAsync(SistemaViewModel modelo, CancellationToken ct = default)
        => _sender.Send(modelo.ToActualizarCommand(), ct);

    public Task EliminarSistemaAsync(int id, CancellationToken ct = default)
        => _sender.Send(new EliminarSistemaCommand(id), ct);

    public Task<IReadOnlyList<ComponenteDto>> ListarComponentesAsync(int activoId, CancellationToken ct = default)
        => _sender.Send(new ListarComponentesQuery(activoId), ct);

    public Task<int> CrearComponenteAsync(ComponenteViewModel modelo, CancellationToken ct = default)
        => _sender.Send(modelo.ToCrearCommand(), ct);

    public Task EliminarComponenteAsync(int id, CancellationToken ct = default)
        => _sender.Send(new EliminarComponenteCommand(id), ct);

    public Task<IReadOnlyList<LocalizacionDto>> ListarLocalizacionesAsync(int? redId = null, CancellationToken ct = default)
        => _sender.Send(new ListarLocalizacionesQuery(redId), ct);

    public Task<IReadOnlyList<RedDto>> ListarRedesAsync(CancellationToken ct = default)
        => _sender.Send(new ListarRedesQuery(), ct);

    public Task<IReadOnlyList<MovimientoDto>> ListarMovimientosAsync(CancellationToken ct = default)
        => _sender.Send(new ListarMovimientosQuery(), ct);

    public Task<IReadOnlyList<MantenimientoDto>> ListarMantenimientosAsync(CancellationToken ct = default)
        => _sender.Send(new ListarMantenimientosQuery(), ct);

    public Task<IReadOnlyList<UsuarioInventarioDto>> ListarUsuariosAsync(CancellationToken ct = default)
        => _sender.Send(new ListarUsuariosQuery(), ct);

    public Task<IReadOnlyList<DocumentoDto>> ListarDocumentosAsync(string entidadTipo, int entidadId, CancellationToken ct = default)
        => _sender.Send(new ListarDocumentosQuery(entidadTipo, entidadId), ct);

    public Task<int> SubirDocumentoAsync(string nombre, string? descripcion, string tipoDocumento, string contentType,
        long tamanoBytes, string entidadTipo, int entidadId, string rutaAlmacenamiento, string? usuario,
        CancellationToken ct = default)
        => _sender.Send(new SubirDocumentoCommand(nombre, descripcion, tipoDocumento, contentType, tamanoBytes,
            entidadTipo, entidadId, rutaAlmacenamiento, usuario), ct);

    public Task<ResultadoImportacion> ImportarSensoresAsync(CancellationToken ct = default)
        => _importacion.ImportarAsync(ct);

    public Task<IReadOnlyList<Inventario.Domain.Entities.Sensor>> ListarSensoresDisponiblesAsync(
        int localizacionId, CancellationToken ct = default)
        => _importacion.ListarSensoresDisponiblesAsync(localizacionId, ct);

    public Task<int> ImportarSensoresAActivoAsync(int activoId, IEnumerable<int> sensorIds, CancellationToken ct = default)
        => _importacion.ImportarSensoresAActivoAsync(activoId, sensorIds, ct);

    public Task MoverComponenteAsync(int componenteId, int nuevoActivoId, CancellationToken ct = default)
        => _importacion.MoverComponenteAsync(componenteId, nuevoActivoId, ct);
}
