namespace Inventario.Domain.ValueObjects;

/// <summary>
/// Coordenadas geográficas de un activo.
/// </summary>
public record Coordenadas
{
    public double Latitud { get; init; }

    public double Longitud { get; init; }

    public Coordenadas(double latitud, double longitud)
    {
        if (latitud is < -90 or > 90)
            throw new ArgumentOutOfRangeException(nameof(latitud), "La latitud debe estar entre -90 y 90.");
        if (longitud is < -180 or > 180)
            throw new ArgumentOutOfRangeException(nameof(longitud), "La longitud debe estar entre -180 y 180.");

        Latitud = latitud;
        Longitud = longitud;
    }

    public override string ToString() => $"{Latitud:F6}, {Longitud:F6}";
}
