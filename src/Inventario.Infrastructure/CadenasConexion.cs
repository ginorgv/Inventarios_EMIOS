using Microsoft.Extensions.Configuration;

namespace Inventario.Infrastructure;

/// <summary>
/// Resuelve las cadenas de conexión a las dos bases (emios301 solo lectura y
/// emios_inventario escritura) de forma configurable:
///   1) Variable de entorno con la cadena COMPLETA
///      (EMIOS301_CONNECTION_STRING / EMIOS_INVENTARIO_CONNECTION_STRING).
///   2) Configuración GRANULAR del servidor: DB_HOST, DB_PORT, DB_USER, DB_PASSWORD
///      (o sección "Db" de appsettings) + nombre de la BD:
///      EMIOS_DB / EMIOS_INVENTARIO_DB
///      (o "Db:Emios301Database" / "Db:EmiosInventarioDatabase" de appsettings).
/// </summary>
public static class CadenasConexion
{
    public static string Emios301(IConfiguration? config = null)
        => Obtener("EMIOS301_CONNECTION_STRING", "EMIOS_DB", "Db:Emios301Database", "emios301", config);

    public static string EmiosInventario(IConfiguration? config = null)
        => Obtener("EMIOS_INVENTARIO_CONNECTION_STRING", "EMIOS_INVENTARIO_DB", "Db:EmiosInventarioDatabase", "emios_inventario", config);

    private static string Obtener(
        string envCompleta,
        string envBaseDatos,
        string claveBd,
        string bdPorDefecto,
        IConfiguration? config)
    {
        // 1) Cadena completa (máxima prioridad).
        var completa = Environment.GetEnvironmentVariable(envCompleta);
        if (!string.IsNullOrWhiteSpace(completa))
            return completa;

        // 2) Configuración granular: servidor + nombre de BD.
        var host = Environment.GetEnvironmentVariable("DB_HOST") ?? config?["Db:Host"] ?? "localhost";
        var port = Environment.GetEnvironmentVariable("DB_PORT") ?? config?["Db:Port"] ?? "3306";
        var user = Environment.GetEnvironmentVariable("DB_USER") ?? config?["Db:User"] ?? "root";
        var pass = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? config?["Db:Password"] ?? "";
        var bd = Environment.GetEnvironmentVariable(envBaseDatos) ?? config?[claveBd] ?? bdPorDefecto;

        return $"Server={host};Port={port};Database={bd};User={user};Password={pass};TreatTinyAsBoolean=true;";
    }
}
