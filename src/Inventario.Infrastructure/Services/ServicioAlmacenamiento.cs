using Microsoft.Extensions.Configuration;

namespace Inventario.Infrastructure.Services;

public interface IServicioAlmacenamiento
{
    Task<string> GuardarAsync(Stream contenido, string nombreFichero, CancellationToken ct = default);
    Task<Stream> LeerAsync(string ruta, CancellationToken ct = default);
    void Eliminar(string ruta);
    string RutaFisica(string ruta);
}

/// <summary>
/// Almacena los ficheros de documentos en el sistema de archivos.
/// La carpeta se configura con la clave "Storage:Ruta" (por defecto wwwroot/uploads).
/// En fases posteriores se puede sustituir por Azure Blob / S3 sin cambiar la interfaz.
/// </summary>
public class ServicioAlmacenamiento : IServicioAlmacenamiento
{
    private readonly string _ruta;

    public ServicioAlmacenamiento(IConfiguration configuration)
    {
        _ruta = configuration["Storage:Ruta"] ?? "wwwroot/uploads";
        Directory.CreateDirectory(Path.GetFullPath(_ruta));
    }

    public async Task<string> GuardarAsync(Stream contenido, string nombreFichero, CancellationToken ct = default)
    {
        var nombreSeguro = Path.GetFileName(nombreFichero);
        var carpeta = DateTime.UtcNow.ToString("yyyy/MM");
        var rutaCompletaDir = Path.Combine(Path.GetFullPath(_ruta), carpeta);
        Directory.CreateDirectory(rutaCompletaDir);

        var nombreUnico = $"{Guid.NewGuid():N}_{nombreSeguro}";
        var rutaFisica = Path.Combine(rutaCompletaDir, nombreUnico);

        await using var fs = File.Create(rutaFisica);
        await contenido.CopyToAsync(fs, ct);

        return Path.Combine(carpeta, nombreUnico).Replace('\\', '/');
    }

    public Task<Stream> LeerAsync(string ruta, CancellationToken ct = default)
        => Task.FromResult<Stream>(File.OpenRead(RutaFisica(ruta)));

    public void Eliminar(string ruta)
    {
        var fisica = RutaFisica(ruta);
        if (File.Exists(fisica))
            File.Delete(fisica);
    }

    public string RutaFisica(string ruta)
        => Path.Combine(Path.GetFullPath(_ruta), ruta.Replace('/', Path.DirectorySeparatorChar));
}
