using Inventario.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Inventario.Infrastructure.Services;

/// <summary>Resultado de la verificación de conectividad con las dos bases de datos.</summary>
public record ResultadoSincronizacion(bool Emios301Ok, string? ErrorEmios301, bool InventarioOk, string? ErrorInventario);

/// <summary>
/// Comprueba la accesibilidad de las bases de datos y prepara la sincronización
/// de la jerarquía (emios301 es la fuente de los niveles 1-2).
/// </summary>
public class ServicioSincronizacion
{
    private readonly IDbContextFactory<EmiosDbContext> _emiosFactory;
    private readonly IDbContextFactory<InventarioDbContext> _inventarioFactory;

    public ServicioSincronizacion(
        IDbContextFactory<EmiosDbContext> emiosFactory,
        IDbContextFactory<InventarioDbContext> inventarioFactory)
    {
        _emiosFactory = emiosFactory;
        _inventarioFactory = inventarioFactory;
    }

    public async Task<ResultadoSincronizacion> VerificarAsync(CancellationToken ct = default)
    {
        // Se crean contextos independientes por llamada (fábrica) para evitar que un
        // componente interactivo (p. ej. TopBar) y la página consulten a la vez sobre
        // la misma instancia scoped de DbContext.
        await using var emios = _emiosFactory.CreateDbContext();
        await using var inventario = _inventarioFactory.CreateDbContext();

        var resultadoEmios = await ProbarAsync(() => emios.Redes.CountAsync(ct));
        var resultadoInventario = await ProbarAsync(() => inventario.Sistemas.CountAsync(ct));

        return new ResultadoSincronizacion(
            resultadoEmios.Ok, resultadoEmios.Error,
            resultadoInventario.Ok, resultadoInventario.Error);
    }

    /// <summary>
    /// Ejecuta la consulta de forma SECUENCIAL y esperada (await). Los contextos son
    /// por ámbito en Blazor Server; lanzar consultas sin esperarlas (fire-and-forget)
    /// provoca "A second operation was started on this context instance...".
    /// </summary>
    private static async Task<(bool Ok, string? Error)> ProbarAsync(Func<Task<int>> consulta)
    {
        try
        {
            await consulta();
            return (true, null);
        }
        catch (Exception ex)
        {
            return (false, ex.Message);
        }
    }
}
