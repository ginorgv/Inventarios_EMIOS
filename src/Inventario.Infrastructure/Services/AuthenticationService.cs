using Inventario.Domain.Entities;
using Inventario.Domain.Interfaces;
using Inventario.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Inventario.Infrastructure.Services;

/// <summary>
/// Valida credenciales contra la tabla <c>usuario</c> de emios301 (solo lectura).
/// </summary>
public class AuthenticationService : IAuthenticationService
{
    private readonly EmiosDbContext _context;
    private readonly IPasswordVerifier _passwordVerifier;

    public AuthenticationService(EmiosDbContext context, IPasswordVerifier passwordVerifier)
    {
        _context = context;
        _passwordVerifier = passwordVerifier;
    }

    public async Task<Usuario?> ValidarCredencialesAsync(string login, string password, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
            return null;

        var usuario = await _context.Usuarios
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == login, ct);

        if (usuario is null)
            return null;

        return _passwordVerifier.Verificar(password, usuario.PasswordHash)
            ? usuario
            : null;
    }

    public async Task<IReadOnlyList<int>> ObtenerRedesAsync(string login, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(login))
            return Array.Empty<int>();

        return await _context.RedesUsuarios
            .AsNoTracking()
            .Where(r => r.Usuario == login)
            .Select(r => r.RedId)
            .ToListAsync(ct);
    }
}
