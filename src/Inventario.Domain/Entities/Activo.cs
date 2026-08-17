using Inventario.Domain.ValueObjects;

namespace Inventario.Domain.Entities;

/// <summary>
/// Nivel 4 de la jerarquía: Activo (nuevo, se gestiona en la nueva BD emios_inventario).
/// </summary>
public class Activo : AuditableEntity
{
    public int Id { get; set; }

    /// <summary>Código de inventario / etiqueta del activo (único).</summary>
    public string Codigo { get; set; } = string.Empty;

    public string Nombre { get; set; } = string.Empty;

    public int SistemaId { get; set; }

    public Sistema? Sistema { get; set; }

    public EstadoActivo Estado { get; set; } = EstadoActivo.Operativo;

    public string? Fabricante { get; set; }

    public string? Modelo { get; set; }

    public string? NumeroSerie { get; set; }

    public DateTime? FechaInstalacion { get; set; }

    /// <summary>Potencia nominal en kW.</summary>
    public decimal? PotenciaNominalKw { get; set; }

    /// <summary>Eficiencia en porcentaje (0-100).</summary>
    public decimal? EficienciaPct { get; set; }

    /// <summary>Fin de garantía.</summary>
    public DateTime? FinGarantia { get; set; }

    /// <summary>Última revisión (mantenimiento realizado).</summary>
    public DateTime? UltimaRevision { get; set; }

    /// <summary>Próxima revisión (mantenimiento previsto).</summary>
    public DateTime? ProximaRevision { get; set; }

    public ICollection<Componente> Componentes { get; set; } = new List<Componente>();

    public ICollection<Documento> Documentos { get; set; } = new List<Documento>();

    public ICollection<Mantenimiento> Mantenimientos { get; set; } = new List<Mantenimiento>();

    public ICollection<Movimiento> Movimientos { get; set; } = new List<Movimiento>();
}
