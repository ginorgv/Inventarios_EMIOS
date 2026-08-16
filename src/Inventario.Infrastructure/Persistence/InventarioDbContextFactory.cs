using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace Inventario.Infrastructure.Persistence;

/// <summary>
/// Fábrica en tiempo de diseño para las migraciones de <see cref="InventarioDbContext"/>.
/// Permite ejecutar "dotnet ef" con Infrastructure como proyecto de inicio sin
/// necesidad de arrancar la aplicación Web.
/// SOLO se usa en diseño (migraciones); en tiempo de ejecución el contexto se
/// registra vía AddDbContext en DependencyInjection.
/// </summary>
public class InventarioDbContextFactory : IDesignTimeDbContextFactory<InventarioDbContext>
{
    public InventarioDbContext CreateDbContext(string[] args)
    {
        // Misma resolución que la aplicación (env vars o valores por defecto).
        var connectionString = CadenasConexion.EmiosInventario();

        var version = ServidorVersion.Resolver(connectionString);

        var options = new DbContextOptionsBuilder<InventarioDbContext>()
            .UseMySql(connectionString, version)
            .Options;

        return new InventarioDbContext(options);
    }
}
