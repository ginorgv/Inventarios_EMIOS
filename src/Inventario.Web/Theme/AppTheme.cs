using MudBlazor;

namespace Inventario.Web.Theme;

/// <summary>
/// Tema MudBlazor con la paleta VERDE definida en el plan:
///   oscuro #298B21 · intermedio #52A652 · claro #5AB55A · fondo #F0F0F0 · fondo_claro #FFFFFF
///   Menú lateral en degradado verde oscuro (#1B3A1B → #0D260D → #1A2E1A).
/// </summary>
public static class AppTheme
{
    public static MudTheme Obtener() => new()
    {
        PaletteLight = new PaletteLight
        {
            // Acento principal (verde).
            Primary = "#298B21",
            PrimaryContrastText = "#FFFFFF",
            Secondary = "#52A652",
            SecondaryContrastText = "#FFFFFF",
            Tertiary = "#5AB55A",
            TertiaryContrastText = "#FFFFFF",

            // Fondos.
            Background = "#EEF1F5",
            BackgroundGray = "#F0F0F0",
            Surface = "#FFFFFF",

            // Menú lateral (verde oscuro degradado).
            DrawerBackground = "#1B3A1B",
            DrawerText = "#FFFFFF",
            DrawerIcon = "#FFFFFF",

            // Barra superior.
            AppbarBackground = "#FFFFFF",
            AppbarText = "#111827",

            // Textos.
            TextPrimary = "#111827",
            TextSecondary = "#4B5563",
            TextDisabled = "#6B7280",

            // Acciones y bordes.
            ActionDefault = "#6B7280",
            LinesDefault = "#D1D5DB",
            LinesInputs = "#D1D5DB",
            TableLines = "#E5E7EB",
            TableHover = "#F3F4F6",
            TableStriped = "#FAFAFA",

            // Estados.
            Success = "#047857",
            SuccessContrastText = "#FFFFFF",
            Error = "#DC2626",
            ErrorContrastText = "#FFFFFF",
            Warning = "#F59E0B",
            WarningContrastText = "#FFFFFF",
            Info = "#1B8FD1",
            InfoContrastText = "#FFFFFF",

            // Superposición de los diálogos (más contraste, menos pálida).
            OverlayLight = "#00000080",
            OverlayDark = "#000000A6",
        },
        LayoutProperties = new LayoutProperties
        {
            DefaultBorderRadius = "8px",
            DrawerWidthLeft = "236px",
            DrawerWidthRight = "236px",
            AppbarHeight = "64px",
        },
        Typography = new Typography
        {
            Default = new DefaultTypography
            {
                FontFamily = new[] { "Segoe UI", "system-ui", "-apple-system", "sans-serif" }
            }
        }
    };
}
