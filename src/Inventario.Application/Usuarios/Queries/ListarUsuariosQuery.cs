using MediatR;
using Inventario.Application.Dtos;
using Inventario.Domain.Entities;
using Inventario.Domain.Interfaces;

namespace Inventario.Application.Usuarios.Queries;

public record ListarUsuariosQuery : IRequest<IReadOnlyList<UsuarioInventarioDto>>;

public class ListarUsuariosQueryHandler : IRequestHandler<ListarUsuariosQuery, IReadOnlyList<UsuarioInventarioDto>>
{
    private readonly IUsuarioInventarioRepository _usuarioRepository;

    public ListarUsuariosQueryHandler(IUsuarioInventarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    public async Task<IReadOnlyList<UsuarioInventarioDto>> Handle(ListarUsuariosQuery request, CancellationToken ct)
    {
        var usuarios = await _usuarioRepository.ObtenerTodosAsync(ct);
        return usuarios.Select(ToDto).ToList();
    }

    internal static UsuarioInventarioDto ToDto(UsuarioInventario u) => new(
        u.Id,
        u.NombreUsuario,
        u.NombreCompleto,
        u.Email,
        u.Rol,
        u.Activo);
}
