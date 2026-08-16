using Inventario.Domain.Entities;
using Inventario.Domain.Interfaces;
using Inventario.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Inventario.Infrastructure.Services;

/// <summary>
/// Jerarquía de 5 niveles filtrada por el usuario logueado:
///   1 Cliente/Proyecto (emios301.clientes)
///   2 Instalación/Site  (emios301.localizaciones, geolocalización)
///   3 Sistema/Subsistema (emios_inventario.sistemas)
///   4 Equipo/Activo Físico (emios_inventario.activos)
///   5 Componente/Sensor (emios_inventario.componentes, enlace a emios301.sensores)
/// La "red" se usa SOLO como filtro (usuario → redes_usuarios → redes → cliente/
/// localización); nunca se devuelve ni se muestra.
/// </summary>
public class ServicioJerarquia
{
    private readonly IDbContextFactory<EmiosDbContext> _emiosFactory;
    private readonly ISistemaRepository _sistemaRepository;
    private readonly IActivoRepository _activoRepository;
    private readonly IComponenteRepository _componenteRepository;

    public ServicioJerarquia(
        IDbContextFactory<EmiosDbContext> emiosFactory,
        ISistemaRepository sistemaRepository,
        IActivoRepository activoRepository,
        IComponenteRepository componenteRepository)
    {
        _emiosFactory = emiosFactory;
        _sistemaRepository = sistemaRepository;
        _activoRepository = activoRepository;
        _componenteRepository = componenteRepository;
    }

    /// <summary>
    /// Clientes (Nivel 1) a los que el usuario tiene acceso: clientes que tienen al
    /// menos una de las redes del usuario.
    /// </summary>
    public async Task<IReadOnlyList<Cliente>> ListarClientesAsync(
        IReadOnlyCollection<int> redIds, CancellationToken ct = default)
    {
        if (redIds.Count == 0)
            return Array.Empty<Cliente>();

        await using var db = _emiosFactory.CreateDbContext();
        return await (from r in db.Redes.AsNoTracking()
                      join c in db.Clientes.AsNoTracking() on r.ClienteId equals c.Id
                      where redIds.Contains(r.Id)
                      select c)
            .Distinct()
            .OrderBy(c => c.Nombre)
            .ToListAsync(ct);
    }

    /// <summary>
    /// Instalaciones/Sites (Nivel 2) de un cliente, restringidas a las redes del usuario.
    /// </summary>
    public async Task<IReadOnlyList<Localizacion>> ListarInstalacionesAsync(
        int clienteId, IReadOnlyCollection<int> redIds, CancellationToken ct = default)
    {
        if (redIds.Count == 0)
            return Array.Empty<Localizacion>();

        await using var db = _emiosFactory.CreateDbContext();

        var redesCliente = await db.Redes.AsNoTracking()
            .Where(r => r.ClienteId == clienteId && redIds.Contains(r.Id))
            .Select(r => r.Id)
            .ToListAsync(ct);

        if (redesCliente.Count == 0)
            return Array.Empty<Localizacion>();

        return await db.Localizaciones.AsNoTracking()
            .Where(l => redesCliente.Contains(l.RedId))
            .OrderBy(l => l.Nombre)
            .ToListAsync(ct);
    }

    /// <summary>Sistemas (Nivel 3) de una instalación.</summary>
    public Task<IReadOnlyList<Sistema>> ListarSistemasAsync(int localizacionId, CancellationToken ct = default)
        => _sistemaRepository.ObtenerPorLocalizacionAsync(localizacionId, ct);

    /// <summary>Activos/Equipos (Nivel 4) de un sistema.</summary>
    public Task<IReadOnlyList<Activo>> ListarActivosAsync(int sistemaId, CancellationToken ct = default)
        => _activoRepository.ObtenerPorSistemaAsync(sistemaId, ct);

    /// <summary>Componentes/Sensores (Nivel 5) de un activo.</summary>
    public Task<IReadOnlyList<Componente>> ListarComponentesAsync(int activoId, CancellationToken ct = default)
        => _componenteRepository.ObtenerPorActivoAsync(activoId, ct);
}
