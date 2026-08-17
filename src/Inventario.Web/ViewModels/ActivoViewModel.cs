using Inventario.Application.Activos.Commands;
using Inventario.Application.Dtos;
using Inventario.Domain.ValueObjects;

namespace Inventario.Web.ViewModels;

/// <summary>Modelo de formulario para crear/editar un Activo (Nivel 4).</summary>
public class ActivoViewModel
{
    public int Id { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public int SistemaId { get; set; }
    public EstadoActivo Estado { get; set; } = EstadoActivo.Operativo;
    public string? Fabricante { get; set; }
    public string? Modelo { get; set; }
    public string? NumeroSerie { get; set; }
    public DateTime? FechaInstalacion { get; set; }
    public decimal? PotenciaNominalKw { get; set; }
    public decimal? EficienciaPct { get; set; }
    public DateTime? FinGarantia { get; set; }
    public DateTime? UltimaRevision { get; set; }
    public DateTime? ProximaRevision { get; set; }

    public static ActivoViewModel FromDto(ActivoDto dto) => new()
    {
        Id = dto.Id,
        Codigo = dto.Codigo,
        Nombre = dto.Nombre,
        SistemaId = dto.SistemaId,
        Estado = dto.Estado,
        Fabricante = dto.Fabricante,
        Modelo = dto.Modelo,
        NumeroSerie = dto.NumeroSerie,
        FechaInstalacion = dto.FechaInstalacion,
        PotenciaNominalKw = dto.PotenciaNominalKw,
        EficienciaPct = dto.EficienciaPct,
        FinGarantia = dto.FinGarantia,
        UltimaRevision = dto.UltimaRevision,
        ProximaRevision = dto.ProximaRevision
    };

    public CrearActivoCommand ToCrearCommand() => new(
        Codigo, Nombre, SistemaId, Estado,
        Fabricante, Modelo, NumeroSerie, FechaInstalacion,
        PotenciaNominalKw, EficienciaPct, FinGarantia, UltimaRevision, ProximaRevision);

    public ActualizarActivoCommand ToActualizarCommand() => new(
        Id, Codigo, Nombre, SistemaId, Estado,
        Fabricante, Modelo, NumeroSerie, FechaInstalacion,
        PotenciaNominalKw, EficienciaPct, FinGarantia, UltimaRevision, ProximaRevision);
}
