using Inventario.Application.Dtos;

namespace Inventario.Web.ViewModels;

/// <summary>Datos agregados que muestra el panel (Dashboard).</summary>
public class DashboardViewModel
{
    public int TotalRedes { get; set; }
    public int TotalLocalizaciones { get; set; }
    public int TotalSistemas { get; set; }
    public int TotalActivos { get; set; }
    public IReadOnlyList<ActivoDto> UltimosActivos { get; set; } = Array.Empty<ActivoDto>();
    public bool Emios301Ok { get; set; }
    public bool InventarioOk { get; set; }
    public string? ErrorEmios301 { get; set; }
    public string? ErrorInventario { get; set; }
}
