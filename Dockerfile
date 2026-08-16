# ============================================================
# Inventario EMIOS — Dockerfile para Railway (y otros hosts)
# Build con .NET 10 SDK, runtime aspnet 10.
# ============================================================

# ---- Etapa de compilación ----
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copia el contexto completo (respeta .dockerignore) y publica solo el Web.
COPY . .
RUN dotnet publish src/Inventario.Web/Inventario.Web.csproj \
    -c Release \
    -o /app/publish \
    --nologo

# ---- Etapa de ejecución ----
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_ENVIRONMENT=Production
EXPOSE 8080

# Railway inyecta la variable PORT; se usa el puerto dinámico con fallback a 8080.
ENTRYPOINT ["sh", "-c", "dotnet Inventario.Web.dll --urls http://0.0.0.0:${PORT:-8080}"]
