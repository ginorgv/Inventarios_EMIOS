# Despliegue en Railway

La app es un **Blazor Server (.NET 10)** con dos bases MariaDB. El despliegue usa el
`Dockerfile` incluido (Railway lo detecta automáticamente; también hay `railway.json`).

## Variables de entorno (imprescindibles)

Puedes configurar el **servidor** y los **nombres de las bases** de dos formas:

### Opción A — granular (recomendada)

| Variable | Descripción |
|---|---|
| `DB_HOST` | Host del MariaDB (p. ej. el que da el plugin de Railway). |
| `DB_PORT` | Puerto (por defecto `3306`). |
| `DB_USER` | Usuario de la base. |
| `DB_PASSWORD` | Contraseña. |
| `EMIOS301_DATABASE` | Nombre del esquema **emios301** (solo lectura). |
| `EMIOS_INVENTARIO_DATABASE` | Nombre del esquema **emios_inventario** (escritura). |
| `PORT` | La define Railway automáticamente (el contenedor escucha en `$PORT`). |

### Opción B — cadena completa

| Variable | Descripción |
|---|---|
| `EMIOS301_CONNECTION_STRING` | Cadena completa a **emios301**. |
| `EMIOS_INVENTARIO_CONNECTION_STRING` | Cadena completa a **emios_inventario**. |

Formato (Pomelo/MySqlConnector):

```
Server=host;Port=3306;Database=emios301;User=usuario;Password=clave;TreatTinyAsBoolean=true;
```

> Prioridad: cadena completa → granular → `appsettings.json` (sección `Db`,
> `localhost` / `root` sin contraseña → solo válido en desarrollo local).

## Bases de datos

- **emios301**: BD heredada de solo lectura. El despliegue en Railway necesita que esta
  BD sea accesible desde el host de Railway (IP pública, VPN/tunnel, o importando los
  datos a la instancia de MariaDB de Railway). **La app nunca la modifica.**
- **emios_inventario**: BD de escritura. **Se crea y migra sola al arrancar**: si el
  esquema no existe, EF lo crea y aplica las migraciones pendientes la primera vez
  (idempotente). No hace falta crearla a mano; basta con que el usuario tenga
  permisos de creación en la instancia MariaDB.

## Pasos rápidos en Railway

1. Crea un proyecto desde el repositorio (GitHub).
2. Railway detecta el `Dockerfile`.
3. Añade un plugin de **MariaDB** (o MySQL) y copia su host/puerto/usuario/clave.
4. (Opcional) Crea el esquema `emios301` en esa instancia (para los datos heredados).
   `emios_inventario` se crea solo.
5. Define las variables de entorno (Opción A o B) en el servicio de la app.
6. Deploy. Healthcheck configurado en `/login`.

## Configuración paso a paso en la UI de Railway

1. Crea un servicio **MySQL** (o MariaDB). Railway genera automáticamente en ese
   servicio las variables `MYSQLHOST`, `MYSQLPORT`, `MYSQLUSER`, `MYSQLPASSWORD`,
   `MYSQLDATABASE`, `MYSQL_URL`.
2. En el **servicio de la app** (pestaña **Variables**) crea estas variables
   (no están predefinidas), referenciando el servicio de BD con `${{Nombre.VAR}}`
   (sustituye `MySQL` por el nombre real de tu servicio de base de datos):

   | Variable | Valor | Nota |
   |---|---|---|
   | `DB_HOST` | `${{MySQL.MYSQLHOST}}` | |
   | `DB_PORT` | `${{MySQL.MYSQLPORT}}` | |
   | `DB_USER` | `${{MySQL.MYSQLUSER}}` | |
   | `DB_PASSWORD` | `${{MySQL.MYSQLPASSWORD}}` | |
   | `EMIOS301_DATABASE` | `emios301` | Esquema heredado (crearlo/importar en la instancia). |
   | `EMIOS_INVENTARIO_DATABASE` | `${{MySQL.MYSQLDATABASE}}` | BD por defecto; la app la crea/migra sola. |

   O alternativamente, con cadenas completas (máxima prioridad):

   ```
   EMIOS301_CONNECTION_STRING = Server=${{MySQL.MYSQLHOST}};Port=${{MySQL.MYSQLPORT}};Database=emios301;User=${{MySQL.MYSQLUSER}};Password=${{MySQL.MYSQLPASSWORD}};TreatTinyAsBoolean=true;
   EMIOS_INVENTARIO_CONNECTION_STRING = Server=${{MySQL.MYSQLHOST}};Port=${{MySQL.MYSQLPORT}};Database=${{MySQL.MYSQLDATABASE}};User=${{MySQL.MYSQLUSER}};Password=${{MySQL.MYSQLPASSWORD}};TreatTinyAsBoolean=true;
   ```

3. Crea el esquema **`emios301`** en la instancia MySQL e importa tus datos heredados
   (la app NO lo crea; es solo lectura). `emios_inventario` se crea y migra sola.
4. Genera un dominio público para la app: **Settings → Networking → Generate Domain**.
5. Deploy. Healthcheck configurado en `/login` (`railway.json`).

> `PORT` la define Railway automáticamente y `ASPNETCORE_ENVIRONMENT=Production` ya
> está fijada en el `Dockerfile`, así que no hace falta configurarlas.

## Notas

- Las migraciones se pueden generar/aplicar localmente con:
  `dotnet ef database update --project src/Inventario.Infrastructure --startup-project src/Inventario.Infrastructure --context InventarioDbContext`
- La subida de archivos usa `wwwroot/uploads/` (filesystem efímero en Railway; no
  persiste entre reinicios). Si se necesita persistencia, apuntar `Storage:Ruta` a un
  volumen/objeto externo.
- Prueba local: `dotnet run --project src/Inventario.Web --urls http://localhost:5099`
