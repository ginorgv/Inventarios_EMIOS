using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace Inventario.Infrastructure;

/// <summary>
/// Resuelve el <see cref="ServerVersion"/> de Pomelo para las dos bases de datos.
/// Detecta automáticamente el servidor real (MySQL o MariaDB y su versión) la
/// primera vez y lo cachea, con fallback a MySQL 8.0 si no se puede detectar
/// (p. ej. BD temporalmente no disponible al arrancar).
/// </summary>
/// <remarks>
/// Motivo: antes la versión estaba fijada a MariaDB 10.11 (ServerType.MariaDb),
/// lo que hace que Pomelo genere <c>INSERT ... RETURNING `Id`</c> (sintaxis de
/// MariaDB ≥ 10.5) y falla cuando la base real es MySQL, que no soporta
/// RETURNING. La detección automática evita ese desajuste.
/// </remarks>
public static class ServidorVersion
{
    private static readonly object _lock = new();
    private static ServerVersion? _cache;

    public static ServerVersion Resolver(string connectionString)
    {
        if (_cache is not null)
            return _cache;

        lock (_lock)
        {
            if (_cache is not null)
                return _cache;

            try
            {
                _cache = ServerVersion.AutoDetect(connectionString);
            }
            catch
            {
                // Fallback compatible con MySQL y MariaDB: MySQL 8.0 no genera
                // RETURNING (usa LAST_INSERT_ID), sintaxis aceptada por ambos.
                _cache = ServerVersion.Create(new Version(8, 0, 0), ServerType.MySql);
            }

            return _cache;
        }
    }
}
