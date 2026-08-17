using Inventario.Domain.Entities;
using Inventario.Domain.Interfaces;

namespace Inventario.Infrastructure.Services;

/// <summary>
/// Resuelve el permiso efectivo de un usuario sobre la jerarquía. Los permisos
/// explícitos se cargan una vez por petición (scoped) y se heredan hacia abajo:
/// el permiso efectivo de un nodo es el máximo entre los permisos explícitos de
/// sus ancestros (global → cliente → localización → sistema → activo) y el propio.
/// </summary>
public class ServicioPermisos
{
    private readonly IPermisoRepository _permisoRepository;
    private Dictionary<(string Tipo, int Id), TipoPermiso> _permisos = new();

    public ServicioPermisos(IPermisoRepository permisoRepository)
    {
        _permisoRepository = permisoRepository;
    }

    /// <summary>Si el usuario tiene algún permiso explícito (si no, se usa el filtro por redes).</summary>
    public bool ConPermisos { get; private set; }

    /// <summary>Si el usuario tiene un permiso global (superusuario).</summary>
    public bool GlobalConcedido => Obtener("global", 0) > TipoPermiso.Ninguno;

    public async Task CargarAsync(string login, CancellationToken ct = default)
    {
        var lista = await _permisoRepository.ObtenerPorUsuarioAsync(login, ct);
        _permisos = new Dictionary<(string, int), TipoPermiso>();
        foreach (var p in lista)
        {
            var clave = (p.EntidadTipo.ToLowerInvariant(), p.EntidadId);
            if (_permisos.TryGetValue(clave, out var actual))
                _permisos[clave] = (TipoPermiso)Math.Max((int)actual, (int)p.TipoPermiso);
            else
                _permisos[clave] = p.TipoPermiso;
        }
        ConPermisos = _permisos.Count > 0;
    }

    public bool ClienteConcedido(int clienteId) => Obtener("cliente", clienteId) > TipoPermiso.Ninguno;
    public bool LocalizacionConcedida(int localizacionId) => Obtener("localizacion", localizacionId) > TipoPermiso.Ninguno;
    public bool SistemaConcedido(int sistemaId) => Obtener("sistema", sistemaId) > TipoPermiso.Ninguno;
    public bool ActivoConcedido(int activoId) => Obtener("activo", activoId) > TipoPermiso.Ninguno;

    /// <summary>Permiso efectivo = máximo entre global, ancestros y el propio nodo.</summary>
    public TipoPermiso PermisoEfectivo(string tipo, int entidadId, params (string Tipo, int Id)[] ancestros)
    {
        var max = Obtener("global", 0);
        foreach (var a in ancestros)
            max = (TipoPermiso)Math.Max((int)max, (int)Obtener(a.Tipo, a.Id));
        max = (TipoPermiso)Math.Max((int)max, (int)Obtener(tipo.ToLowerInvariant(), entidadId));
        return max;
    }

    /// <summary>Puede crear/editar (permiso efectivo ≥ Edicion).</summary>
    public bool PuedeEditar(string tipo, int entidadId, params (string Tipo, int Id)[] ancestros)
        => PermisoEfectivo(tipo, entidadId, ancestros) >= TipoPermiso.Edicion;

    /// <summary>Puede eliminar/gestionar (permiso efectivo ≥ Administracion).</summary>
    public bool PuedeAdministrar(string tipo, int entidadId, params (string Tipo, int Id)[] ancestros)
        => PermisoEfectivo(tipo, entidadId, ancestros) >= TipoPermiso.Administracion;

    private TipoPermiso Obtener(string tipo, int id)
        => _permisos.TryGetValue((tipo, id), out var t) ? t : TipoPermiso.Ninguno;
}
