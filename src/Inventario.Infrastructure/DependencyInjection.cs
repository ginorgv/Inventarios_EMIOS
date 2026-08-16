using Inventario.Domain.Interfaces;
using Inventario.Infrastructure.Persistence;
using Inventario.Infrastructure.Repositories;
using Inventario.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace Inventario.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Cadenas de conexión configurables (variables de entorno o sección "Db"
        // de appsettings): servidor y nombres de base de datos.
        var csEmios = CadenasConexion.Emios301(configuration);
        var csInventario = CadenasConexion.EmiosInventario(configuration);

        // Versión de MariaDB usada para generar SQL. Se usa explícita (y no
        // ServerVersion.AutoDetect) para no abrir una conexión al construir
        // las opciones del contexto. Ajustar a la versión real si es necesario.
        var versionServidor = ServerVersion.Create(new Version(10, 11, 0), ServerType.MariaDb);

        // Contexto de SOLO LECTURA sobre emios301 (no se migra ni se modifica).
        // Se registra como fábrica (IDbContextFactory) para que los componentes
        // interactivos de Blazor Server puedan consultar en paralelo sin conflictos
        // de concurrencia. AddDbContextFactory también registra el contexto como
        // scoped (misma instancia por ámbito), por lo que la inyección directa de
        // EmiosDbContext en servicios/repositorios sigue funcionando.
        services.AddDbContextFactory<EmiosDbContext>(options =>
            options.UseMySql(csEmios, versionServidor));

        // Contexto de ESCRITURA sobre emios_inventario (migraciones EF).
        services.AddDbContextFactory<InventarioDbContext>(options =>
            options.UseMySql(csInventario, versionServidor)
                .AddInterceptors(new AuditoriaInterceptor()));

        // Repositorios.
        services.AddScoped<IRedRepository, RedRepository>();
        services.AddScoped<ILocalizacionRepository, LocalizacionRepository>();
        services.AddScoped<ISistemaRepository, SistemaRepository>();
        services.AddScoped<IActivoRepository, ActivoRepository>();
        services.AddScoped<IComponenteRepository, ComponenteRepository>();
        services.AddScoped<IDocumentoRepository, DocumentoRepository>();
        services.AddScoped<IMovimientoRepository, MovimientoRepository>();
        services.AddScoped<IMantenimientoRepository, MantenimientoRepository>();
        services.AddScoped<IUsuarioInventarioRepository, UsuarioInventarioRepository>();
        services.AddScoped<IClienteDatosRepository, ClienteDatosRepository>();
        services.AddScoped<IPermisoRepository, PermisoRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Servicios.
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IPasswordVerifier, LegacyPasswordVerifier>();
        services.AddScoped<IServicioAlmacenamiento, ServicioAlmacenamiento>();
        services.AddScoped<ServicioSincronizacion>();
        services.AddScoped<ServicioImportacionSensores>();
        services.AddScoped<ServicioJerarquia>();
        services.AddScoped<ServicioClienteDatos>();

        return services;
    }
}
