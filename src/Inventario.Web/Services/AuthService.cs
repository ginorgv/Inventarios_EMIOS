using System.Security.Claims;
using Inventario.Domain.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using IAuthenticationService = Inventario.Domain.Interfaces.IAuthenticationService;

namespace Inventario.Web.Services;

public interface IAuthService
{
    Task<Usuario?> ValidarAsync(string login, string password, CancellationToken ct = default);
    Task IniciarSesionAsync(Usuario usuario, bool recuerdame, HttpContext httpContext, CancellationToken ct = default);
    Task CerrarSesionAsync(HttpContext httpContext);
}

/// <summary>
/// Gestión de inicio/cierre de sesión con cookies, autenticando contra la
/// tabla <c>usuario</c> de emios301 a través de <see cref="IAuthenticationService"/>.
/// </summary>
public class AuthService : IAuthService
{
    private readonly IAuthenticationService _authenticationService;

    public AuthService(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    public Task<Usuario?> ValidarAsync(string login, string password, CancellationToken ct = default)
        => _authenticationService.ValidarCredencialesAsync(login, password, ct);

    public async Task IniciarSesionAsync(Usuario usuario, bool recuerdame, HttpContext httpContext, CancellationToken ct = default)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, usuario.Id),
            new(ClaimTypes.NameIdentifier, usuario.Id),
            new(ClaimTypes.GivenName, usuario.Nombre ?? usuario.Id),
            new(ClaimTypes.Role, usuario.Perfil ?? "Consulta")
        };

        // Multi-tenant: las redes del usuario se guardan en un claim para filtrar
        // clientes/localizaciones. La red es solo lógica, nunca se muestra.
        var redes = await _authenticationService.ObtenerRedesAsync(usuario.Id, ct);
        if (redes.Count > 0)
            claims.Add(new Claim("Redes", string.Join(",", redes)));

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        await httpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(identity),
            new AuthenticationProperties
            {
                IsPersistent = recuerdame,
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(recuerdame ? 30 : 8)
            });
    }

    public Task CerrarSesionAsync(HttpContext httpContext)
        => httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
}
