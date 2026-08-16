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
        // Misma cadena de conexión y versión de servidor que appsettings.json.
        var connectionString = Environment.GetEnvironmentVariable("EMIOS_INVENTARIO_CS")
            ?? "Server=localhost;Port=3306;Database=emios_inventario;User=root;Password=;TreatTinyAsBoolean=true;";

        var version = ServerVersion.Create(new Version(10, 11, 0), ServerType.MariaDb);

        var options = new DbContextOptionsBuilder<InventarioDbContext>()
            .UseMySql(connectionString, version)
            .Options;

        return new InventarioDbContext(options);
    }
}
