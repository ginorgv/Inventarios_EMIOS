InventarioApplus.sln
├── src/
│   ├── Inventario.Domain/                    # Núcleo
│   │   ├── Entities/
│   │   │   ├── Red.cs                        # Nivel 1 (vista read-only)
│   │   │   ├── Localizacion.cs               # Nivel 2 (vista read-only)
│   │   │   ├── Sistema.cs                    # Nivel 3 (nuevo)
│   │   │   ├── Activo.cs                     # Nivel 4 (nuevo)
│   │   │   ├── Componente.cs                 # Nivel 5 (nuevo)
│   │   │   ├── Documento.cs                  
│   │   │   ├── Mantenimiento.cs
│   │   │   ├── Movimiento.cs
│   │   │   └── UsuarioInventario.cs
│   │   ├── ValueObjects/
│   │   │   ├── Coordenadas.cs
│   │   │   ├── EstadoActivo.cs
│   │   │   └── RangoMedicion.cs
│   │   └── Interfaces/
│   │       ├── IRedRepository.cs
│   │       ├── ILocalizacionRepository.cs
│   │       ├── ISistemaRepository.cs
│   │       ├── IActivoRepository.cs
│   │       ├── IComponenteRepository.cs
│   │       ├── IDocumentoRepository.cs
│   │       ├── IPermisoRepository.cs
│   │       └── IUnitOfWork.cs
│   │
│   ├── Inventario.Application/               # Casos de uso
│   │   ├── Activos/
│   │   │   ├── Commands/
│   │   │   │   ├── CrearActivoCommand.cs
│   │   │   │   ├── ActualizarActivoCommand.cs
│   │   │   │   └── EliminarActivoCommand.cs
│   │   │   └── Queries/
│   │   │       ├── ObtenerActivoQuery.cs
│   │   │       └── ListarActivosQuery.cs
│   │   ├── Arbol/
│   │   │   ├── Commands/
│   │   │   │   └── SincronizarArbolCommand.cs
│   │   │   └── Queries/
│   │   │       └── ObtenerArbolCompletoQuery.cs
│   │   ├── Documentos/
│   │   │   ├── Commands/
│   │   │   │   └── SubirDocumentoCommand.cs
│   │   │   └── Queries/
│   │   │       └── ListarDocumentosQuery.cs
│   │   ├── Dtos/
│   │   │   ├── ActivoDto.cs
│   │   │   ├── SistemaDto.cs
│   │   │   ├── ArbolDto.cs
│   │   │   └── DocumentoDto.cs
│   │   ├── Mappings/
│   │   │   └── AutoMapperProfile.cs
│   │   └── Common/
│   │       ├── Validators/
│   │       └── Exceptions/
│   │
│   ├── Inventario.Infrastructure/            # Implementaciones
│   │   ├── Persistence/
│   │   │   ├── InventarioDbContext.cs        # Contexto principal
│   │   │   ├── EmiosDbContext.cs             # Contexto read-only
│   │   │   └── Configurations/
│   │   │       ├── RedConfiguration.cs
│   │   │       ├── LocalizacionConfiguration.cs
│   │   │       ├── SistemaConfiguration.cs
│   │   │       └── ActivoConfiguration.cs
│   │   ├── Repositories/
│   │   │   ├── RedRepository.cs
│   │   │   ├── LocalizacionRepository.cs
│   │   │   ├── SistemaRepository.cs
│   │   │   ├── ActivoRepository.cs
│   │   │   ├── ComponenteRepository.cs
│   │   │   └── DocumentoRepository.cs
│   │   ├── Services/
│   │   │   ├── ServicioSincronizacion.cs
│   │   │   ├── ServicioAlmacenamiento.cs    # Documentos
│   │   │   └── ServicioAuditoria.cs
│   │   ├── Identity/
│   │   │   ├── AppUser.cs
│   │   │   └── IdentityDbContext.cs
│   │   └── DependencyInjection.cs
│   │
│   └── Inventario.Web/                       # Blazor Server
│       ├── Components/
│       │   ├── Layout/
│       │   │   ├── MainLayout.razor
│       │   │   └── NavMenu.razor
│       │   ├── Shared/
│       │   │   ├── ArbolJerarquico.razor    
│       │   │   ├── MapaInstalaciones.razor  
│       │   │   └── SelectorSensor.razor     
│       │   └── Forms/
│       │       ├── FormularioNivel3.razor
│       │       ├── FormularioNivel4.razor
│       │       └── FormularioNivel5.razor
│       ├── Pages/
│       │   ├── Activos/
│       │   │   ├── Index.razor
│       │   │   ├── Detalle.razor
│       │   │   ├── Crear.razor
│       │   │   └── Editar.razor
│       │   ├── Arbol/
│       │   │   └── Visualizador.razor
│       │   ├── Dashboard/
│       │   │   └── Index.razor
│       │   └── Auth/
│       │       ├── Login.razor
│       │       └── Logout.razor
│       ├── ViewModels/
│       │   ├── ActivoViewModel.cs
│       │   └── ArbolViewModel.cs
│       ├── Services/
│       │   ├── IArbolService.cs
│       │   └── IActivoService.cs
│       ├── wwwroot/
│       │   ├── css/
│       │   │   └── site.css
│       │   ├── images/
│       │   └── lib/
│       ├── Program.cs
│       └── appsettings.json
│
└── tests/
    ├── Inventario.Domain.Tests/
    └── Inventario.Application.Tests/