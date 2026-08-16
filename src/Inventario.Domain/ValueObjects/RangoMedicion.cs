namespace Inventario.Domain.ValueObjects;

/// <summary>
/// Rango de medición de un componente/sensor.
/// </summary>
public record RangoMedicion
{
    public decimal Minimo { get; init; }

    public decimal Maximo { get; init; }

    public string Unidad { get; init; } = string.Empty;

    public RangoMedicion(decimal minimo, decimal maximo, string unidad)
    {
        if (maximo < minimo)
            throw new ArgumentException("El máximo no puede ser menor que el mínimo.");

        Minimo = minimo;
        Maximo = maximo;
        Unidad = unidad;
    }

    public override string ToString() => $"{Minimo} - {Maximo} {Unidad}";
}
