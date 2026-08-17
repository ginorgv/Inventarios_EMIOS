using MudBlazor;

namespace Inventario.Web.ViewModels;

/// <summary>
/// Opciones reutilizables de diálogo para mantener una apariencia coherente:
/// ancho compacto, botón de cerrar, escape y bloqueo del clic fuera del diálogo.
/// </summary>
public static class Dialogos
{
    /// <summary>Formularios (Sistema, Activo, ClienteDatos, MoverSensor).</summary>
    public static DialogOptions Formulario() => new()
    {
        MaxWidth = MaxWidth.Small,
        FullWidth = true,
        CloseButton = true,
        CloseOnEscapeKey = true
    };

    /// <summary>Diálogos con tabla (Importar sensores).</summary>
    public static DialogOptions Tabla() => new()
    {
        MaxWidth = MaxWidth.Medium,
        FullWidth = true,
        CloseButton = true,
        CloseOnEscapeKey = true
    };

    /// <summary>Confirmaciones (MessageBox).</summary>
    public static DialogOptions Confirmacion() => new()
    {
        CloseButton = true,
        CloseOnEscapeKey = true
    };
}
