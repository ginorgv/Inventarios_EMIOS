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

// En despliegues (Railway/Producción) con conexión por variable de entorno, se
// aplican las migraciones de emios_inventario al arrancar (idempotente). En
// desarrollo local se omiten porque la BD ya está migrada.
if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("EMIOS_INVENTARIO_CONNECTION_STRING")))
{
    using var scope = app.Services.CreateScope();
    try
    {
        var factory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<InventarioDbContext>>();
        using var ctx = factory.CreateDbContext();
        await ctx.Database.MigrateAsync();
        app.Logger.LogInformation("Migraciones de emios_inventario aplicadas correctamente.");
    }
    catch (Exception ex)
    {
        app.Logger.LogWarning(ex, "No se pudieron aplicar las migraciones de emios_inventario al arrancar.");
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

app.Run();

/// <summary>Petición de inicio de sesión para el endpoint /api/login.</summary>
public record LoginRequest(string Login, string Password, bool Recordar);
