using Inventario.Domain.Entities;
using Inventario.Domain.Interfaces;
using Inventario.Domain.ValueObjects;
using Inventario.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Inventario.Infrastructure.Services;

/// <summary>Resultado de la importación de sensores como Componentes (nivel 5).</summary>
public record ResultadoImportacion(
    int SensoresTotal,
    int ActivosCreados,
    int ActivosActualizados,
    int ComponentesCreados,
    int ComponentesActualizados,
    int SinLocalizacion);

/// <summary>
/// Importa los sensores de emios301 (solo lectura) como Componentes (nivel 5) en
/// emios_inventario. Por cada sensor se crea/actualiza un Activo/equipo (nivel 4)
/// de respaldo y su Componente vinculado (Componente.SensorId = id del sensor).
/// Después se pueden mover los sensores (componentes) a otros activos o crear otros
/// equipos desde la interfaz. Es idempotente: si el sensor ya está importado,
/// actualiza el activo y el componente sin duplicar.
/// </summary>
public class ServicioImportacionSensores
{
    private readonly EmiosDbContext _emios;
    private readonly ISistemaRepository _sistemaRepository;
    private readonly IActivoRepository _activoRepository;
    private readonly IComponenteRepository _componenteRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ServicioImportacionSensores(
        EmiosDbContext emios,
        ISistemaRepository sistemaRepository,
        IActivoRepository activoRepository,
        IComponenteRepository componenteRepository,
        IUnitOfWork unitOfWork)
    {
        _emios = emios;
        _sistemaRepository = sistemaRepository;
        _activoRepository = activoRepository;
        _componenteRepository = componenteRepository;
        _unitOfWork = unitOfWork;
    }

    /// <summary>Localización especial para sensores sin localización válida.</summary>
    private const int SinLocalizacionId = 0;

    public async Task<ResultadoImportacion> ImportarAsync(CancellationToken ct = default)
    {
        var sensores = await _emios.Sensores.OrderBy(s => s.Id).ToListAsync(ct);
        var localizaciones = await _emios.Localizaciones.AsNoTracking().ToListAsync(ct);
        var locIds = localizaciones.Select(l => l.Id).ToHashSet();

        // Sistemas existentes agrupados por localización (nivel 3).
        var sistemas = await _sistemaRepository.ObtenerTodosAsync(ct);
        var sistemaPorLoc = sistemas
            .GroupBy(s => s.LocalizacionId)
            .ToDictionary(g => g.Key, g => g.First());

        // Garantizar el sistema "sin localización" (fallback).
        if (!sistemaPorLoc.ContainsKey(SinLocalizacionId))
        {
            var fallback = new Sistema
            {
                Codigo = "SYS-SIN-LOC",
                Nombre = "Sin localización",
                Descripcion = "Sistema automático para sensores sin localización asignada (importación).",
                LocalizacionId = SinLocalizacionId,
                Activo = true
            };
            await _sistemaRepository.AgregarAsync(fallback, ct);
            sistemaPorLoc[SinLocalizacionId] = fallback;
        }

        // Fase 1: crear los sistemas por localización que falten y persistirlos
        // para disponer de sus Ids antes de enlazar los activos.
        var localizacionesNecesarias = sensores
            .Select(s => locIds.Contains(s.LocalizacionId) ? s.LocalizacionId : SinLocalizacionId)
            .Distinct();
        foreach (var locId in localizacionesNecesarias)
        {
            if (sistemaPorLoc.ContainsKey(locId))
                continue;

            var loc = localizaciones.FirstOrDefault(l => l.Id == locId);
            var sistema = new Sistema
            {
                Codigo = $"SYS-{locId}",
                Nombre = loc?.Nombre ?? $"Sistema {locId}",
                Descripcion = "Sistema automático creado en la importación de sensores.",
                LocalizacionId = locId,
                Activo = true
            };
            await _sistemaRepository.AgregarAsync(sistema, ct);
            sistemaPorLoc[locId] = sistema;
        }
        await _unitOfWork.SaveChangesAsync(ct);

        // Fase 2: upsert de Activos (equipos, nivel 4) y Componentes (sensores, nivel 5).
        var activos = await _activoRepository.ObtenerTodosAsync(ct);
        var activoPorId = activos.ToDictionary(a => a.Id);
        var componentes = await _componenteRepository.ObtenerTodosAsync(ct);
        var compPorSensor = componentes
            .ToDictionary(c => c.SensorId);

        var creadosA = 0; var actualizadosA = 0;
        var creadosC = 0; var actualizadosC = 0;
        var sinLoc = 0;

        foreach (var sensor in sensores)
        {
            var locId = locIds.Contains(sensor.LocalizacionId) ? sensor.LocalizacionId : SinLocalizacionId;
            if (locId == SinLocalizacionId)
                sinLoc++;

            var sistema = sistemaPorLoc[locId];

            if (compPorSensor.TryGetValue(sensor.Id, out var comp))
            {
                // Actualizar el componente (sensor) y su activo (equipo).
                comp.Nombre = sensor.Nombre;
                comp.Tipo = sensor.Clase;
                if (string.IsNullOrWhiteSpace(comp.Descripcion))
                    comp.Descripcion = sensor.Descripcion;
                _componenteRepository.Actualizar(comp);
                actualizadosC++;

                if (activoPorId.TryGetValue(comp.ActivoId, out var activo))
                {
                    activo.Nombre = sensor.Nombre;
                    activo.TipoActivo = sensor.Clase;
                    activo.SistemaId = sistema.Id;
                    _activoRepository.Actualizar(activo);
                    actualizadosA++;
                }
            }
            else
            {
                // Crear Activo (equipo) + Componente (sensor). La relación ActivoId se
                // fija por navegación (activo.Componentes) al guardar.
                var activo = new Activo
                {
                    Codigo = $"EQ-{sensor.Id:00000}",
                    Nombre = sensor.Nombre,
                    Descripcion = sensor.Descripcion,
                    SistemaId = sistema.Id,
                    TipoActivo = sensor.Clase,
                    Estado = EstadoActivo.Activo
                };
                activo.Componentes.Add(new Componente
                {
                    Codigo = $"SEN-{sensor.Id:00000}",
                    Nombre = sensor.Nombre,
                    Tipo = sensor.Clase,
                    Descripcion = sensor.Descripcion,
                    SensorId = sensor.Id
                });
                await _activoRepository.AgregarAsync(activo, ct);
                activoPorId[activo.Id] = activo;
                creadosA++;
                creadosC++;
            }
        }

        await _unitOfWork.SaveChangesAsync(ct);
        return new ResultadoImportacion(sensores.Count, creadosA, actualizadosA, creadosC, actualizadosC, sinLoc);
    }

    /// <summary>
    /// Sensores de una localización (geolocalización) que aún NO están asignados a
    /// ningún componente, para poder añadirlos a un activo concreto.
    /// </summary>
    public async Task<IReadOnlyList<Sensor>> ListarSensoresDisponiblesAsync(
        int localizacionId, CancellationToken ct = default)
    {
        var componentes = await _componenteRepository.ObtenerTodosAsync(ct);
        var usados = componentes
            .Select(c => c.SensorId)
            .ToHashSet();

        var sensores = await _emios.Sensores
            .AsNoTracking()
            .Where(s => s.LocalizacionId == localizacionId)
            .OrderBy(s => s.Nombre)
            .ToListAsync(ct);

        return sensores.Where(s => !usados.Contains(s.Id)).ToList();
    }

    /// <summary>
    /// Añade los sensores elegidos como Componentes (nivel 5) al activo indicado,
    /// copiando los datos del sensor de emios301. Devuelve cuántos se crearon.
    /// </summary>
    public async Task<int> ImportarSensoresAActivoAsync(
        int activoId, IEnumerable<int> sensorIds, CancellationToken ct = default)
    {
        var ids = sensorIds.Distinct().ToList();
        if (ids.Count == 0)
            return 0;

        var componentes = await _componenteRepository.ObtenerTodosAsync(ct);
        var usados = componentes
            .Select(c => c.SensorId)
            .ToHashSet();

        var sensores = await _emios.Sensores
            .AsNoTracking()
            .Where(s => ids.Contains(s.Id))
            .ToListAsync(ct);

        var creados = 0;
        foreach (var sensor in sensores.OrderBy(s => s.Id))
        {
            if (usados.Contains(sensor.Id))
                continue; // ya asignado a otro activo

            var componente = new Componente
            {
                Codigo = $"SEN-{sensor.Id:00000}",
                Nombre = sensor.Nombre,
                Tipo = sensor.Clase,
                Descripcion = sensor.Descripcion,
                SensorId = sensor.Id,
                ActivoId = activoId
            };
            await _componenteRepository.AgregarAsync(componente, ct);
            usados.Add(sensor.Id);
            creados++;
        }

        if (creados > 0)
            await _unitOfWork.SaveChangesAsync(ct);

        return creados;
    }

    /// <summary>Mueve un componente/sensor de un activo a otro (cambia su ActivoId).</summary>
    public async Task MoverComponenteAsync(int componenteId, int nuevoActivoId, CancellationToken ct = default)
    {
        var componente = await _componenteRepository.ObtenerPorIdAsync(componenteId, ct)
            ?? throw new InvalidOperationException($"El componente {componenteId} no existe.");

        if (componente.ActivoId == nuevoActivoId)
            return;

        componente.ActivoId = nuevoActivoId;
        _componenteRepository.Actualizar(componente);
        await _unitOfWork.SaveChangesAsync(ct);
    }
}
