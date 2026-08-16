using Inventario.Application.Dtos;
using Inventario.Domain.Entities;

namespace Inventario.Application.Mappings;

/// <summary>
/// Construcción del árbol jerárquico Red → Localización → Sistema → Activo → Componente.
/// Red y Localización provienen de emios301 (solo lectura); el resto de emios_inventario.
/// </summary>
public static class ArbolMappings
{
    public static List<ArbolDto> Construir(
        IReadOnlyList<Red> redes,
        IReadOnlyList<Localizacion> localizaciones,
        IReadOnlyList<Sistema> sistemas,
        IReadOnlyList<Activo> activos,
        IReadOnlyList<Componente> componentes)
    {
        var componentesPorActivo = componentes.GroupBy(c => c.ActivoId)
            .ToDictionary(g => g.Key, g => g.ToList());

        var activosPorSistema = activos.GroupBy(a => a.SistemaId)
            .ToDictionary(g => g.Key, g => g.ToList());

        var sistemasPorLocalizacion = sistemas.GroupBy(s => s.LocalizacionId)
            .ToDictionary(g => g.Key, g => g.ToList());

        var localizacionesPorRed = localizaciones.GroupBy(l => l.RedId)
            .ToDictionary(g => g.Key, g => g.ToList());

        var arbol = new List<ArbolDto>();
        foreach (var red in redes)
        {
            var nodoRed = Nodo(1, "Red", red.Id, "", red.Nombre, null);
            arbol.Add(nodoRed);

            if (!localizacionesPorRed.TryGetValue(red.Id, out var locs))
                continue;

            foreach (var loc in locs)
            {
                var nodoLoc = Nodo(2, "Localizacion", loc.Id, "", loc.Nombre, null);
                nodoRed.Hijos.Add(nodoLoc);

                if (!sistemasPorLocalizacion.TryGetValue(loc.Id, out var sists))
                    continue;

                foreach (var sis in sists)
                {
                    var nodoSis = Nodo(3, "Sistema", sis.Id, sis.Codigo, sis.Nombre,
                        sis.Activo ? "Activo" : "Inactivo");
                    nodoLoc.Hijos.Add(nodoSis);

                    if (!activosPorSistema.TryGetValue(sis.Id, out var acts))
                        continue;

                    foreach (var act in acts)
                    {
                        var nodoAct = Nodo(4, "Activo", act.Id, act.Codigo, act.Nombre, act.Estado.ToString());
                        nodoSis.Hijos.Add(nodoAct);

                        if (componentesPorActivo.TryGetValue(act.Id, out var comps))
                        {
                            foreach (var comp in comps)
                                nodoAct.Hijos.Add(Nodo(5, "Componente", comp.Id, comp.Codigo, comp.Nombre, comp.Tipo));
                        }
                    }
                }
            }
        }

        return arbol;
    }

    private static ArbolDto Nodo(int nivel, string tipo, int id, string codigo, string nombre, string? estado) =>
        new()
        {
            Nivel = nivel,
            Tipo = tipo,
            Id = id,
            Codigo = codigo,
            Nombre = nombre,
            Estado = estado
        };
}
