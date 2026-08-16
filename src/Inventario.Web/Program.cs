using Inventario.Application;
using Inventario.Infrastructure;
using Inventario.Infrastructure.Persistence;
using Inventario.Web.Components;
using Inventario.Web.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Servicios de la aplicación (Clean Architecture).
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// Blazor Server (interactividad en servidor).
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// MudBlazor (componentes UI).
builder.Services.AddMudServices();

// Autenticación con cookies contra la tabla `usuario` de emios301.
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/login";
        options.AccessDeniedPath = "/login";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;
    });
builder.Services.AddAuthorization();
builder.Services.AddHttpContextAccessor();

// Servicios de la capa de presentación.
builder.Services.AddScoped<IActivoService, ActivoService>();
builder.Services.AddScoped<IArbolService, ArbolService>();
builder.Services.AddScoped<IAuthService, AuthService>();

var app = builder.Build();

// Asegura que emios_inventario exista y esté migrada (idempotente). Si la base no
// existe, EF la crea y aplica las migraciones pendientes la primera vez. emios301
// (solo lectura) no se migra nunca.
using (var scope = app.Services.CreateScope())
{
    try
    {
        var factory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<InventarioDbContext>>();
        using var ctx = factory.CreateDbContext();
        await ctx.Database.MigrateAsync();
        app.Logger.LogInformation("Migraciones de emios_inventario aplicadas correctamente.");
    }
    catch (Exception ex)
    {
        app.Logger.LogWarning(ex, "No se pudieron aplicar las migraciones de emios_inventario al arrancar (¿BD no disponible?).");
    }
}

// Pipeline HTTP.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);

app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Endpoint de inicio de sesión. Se firma la cookie aquí (contexto HTTP normal)
// en lugar de hacerlo dentro del circuito interactivo, donde la respuesta ya se
// habría iniciado y SignInAsync lanzaría "Headers are read-only".
app.MapPost("/api/login", async (
        LoginRequest req,
        HttpContext http,
        IAuthService auth,
        CancellationToken ct) =>
    {
        var usuario = await auth.ValidarAsync(req.Login, req.Password, ct);
        if (usuario is null)
            return Results.Unauthorized();

        await auth.IniciarSesionAsync(usuario, req.Recordar, http, ct);
        return Results.Ok();
    })
    .AllowAnonymous();

// Cierre de sesión en contexto HTTP normal (no dentro del circuito interactivo,
// donde SignOutAsync fallaría por "Headers are read-only, response has already started").
app.MapGet("/logout", async (HttpContext http, IAuthService auth) =>
{
    await auth.CerrarSesionAsync(http);
    return Results.Redirect("/sesion-cerrada");
});

app.Run();

/// <summary>Petición de inicio de sesión para el endpoint /api/login.</summary>
public record LoginRequest(string Login, string Password, bool Recordar);
