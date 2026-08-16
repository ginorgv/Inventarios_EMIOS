# Despliegue en Railway

La app es un **Blazor Server (.NET 10)** con dos bases MariaDB. El despliegue usa el
`Dockerfile` incluido (Railway lo detecta automáticamente; también hay `railway.json`).

## Variables de entorno (imprescindibles)

| Variable | Descripción |
|---|---|
| `EMIOS301_CONNECTION_STRING` | Cadena de conexión a **emios301** (solo lectura, BD heredada). |
| `EMIOS_INVENTARIO_CONNECTION_STRING` | Cadena de conexión a **emios_inventario** (escritura + migraciones EF). |
| `PORT` | La define Railway automáticamente (el contenedor escucha en `$PORT`). |

Formato de cadena (Pomelo/MySqlConnector):

```
Server=host;Port=3306;Database=emios301;User=usuario;Password=clave;TreatTinyAsBoolean=true;
```

> Si no se definen las variables, se usan los valores de `appsettings.json`
> (`localhost`, usuario `root` sin contraseña) → **solo válido en desarrollo local**.

## Bases de datos

- **emios301**: BD heredada de solo lectura. El despliegue en Railway necesita que esta
  BD sea accesible desde el host de Railway (IP pública, VPN/tunnel, o importando los
  datos a la instancia de MariaDB de Railway). **La app nunca la modifica.**
- **emios_inventario**: BD de escritura. **Las migraciones EF se aplican solas al
  arrancar** cuando está definida `EMIOS_INVENTARIO_CONNECTION_STRING` (idempotente).
  En Railway conviene crear un servicio **MariaDB** y crear los dos esquemas
  (`emios301` y `emios_inventario`) en la misma instancia.

## Pasos rápidos en Railway

1. Crea un proyecto desde el repositorio (GitHub).
2. Railway detecta el `Dockerfile`.
3. Añade un plugin de **MariaDB** (o MySQL) y copia su cadena de conexión.
4. Crea los esquemas `emios301` y `emios_inventario` en esa instancia.
5. Define `EMIOS301_CONNECTION_STRING` y `EMIOS_INVENTARIO_CONNECTION_STRING`
   en las variables de entorno del servicio de la app.
6. Deploy. Healthcheck configurado en `/login`.

## Notas

- Las migraciones se pueden generar/aplicar localmente con:
  `dotnet ef database update --project src/Inventario.Infrastructure --startup-project src/Inventario.Infrastructure --context InventarioDbContext`
- La subida de archivos usa `wwwroot/uploads/` (filesystem efímero en Railway; no
  persiste entre reinicios). Si se necesita persistencia, apuntar `Storage:Ruta` a un
  volumen/objeto externo.
- Prueba local: `dotnet run --project src/Inventario.Web --urls http://localhost:5099`
