# ConsoleCLILibrary2

Biblioteca de .NET para servicios de despliegue (DeployService).

## ?? Publicación Local de NuGet Package

Este proyecto está configurado para publicar paquetes NuGet de forma **local** en el directorio `nupkg`.

### Requisitos Previos

- .NET SDK 10.0 o superior
- Configuración en `NuGet.config` con la fuente local habilitada

### Paso 1: Generar el Paquete NuGet

Desde la raíz del proyecto, ejecuta:

```bash
dotnet pack ConsoleCLILibrary2/ConsoleCLILibrary2.csproj --configuration Release
```

Esto generará el archivo `.nupkg` en el directorio `nupkg/` de la raíz del workspace.

### Paso 2: Verificar el Paquete Generado

```bash
ls nupkg/
```

Deberías ver un archivo como: `ConsoleCLILibrary2.1.0.0.nupkg`

### Paso 3: Configurar NuGet.config (si no está configurado)

Asegúrate de que el archivo `NuGet.config` en la raíz tenga habilitada la fuente local:

```xml
<packageSources>
  <add key="local" value="./nupkg" />
  <add key="nuget.org" value="https://api.nuget.org/v3/index.json" protocolVersion="3" />
</packageSources>
```

### Paso 4: Usar el Paquete en Otro Proyecto

#### Opción A: Agregar referencia directa

```bash
dotnet add package ConsoleCLILibrary2 --version 1.0.0
```

#### Opción B: Especificar la fuente local

```bash
dotnet add package ConsoleCLILibrary2 --version 1.0.0 --source ./nupkg
```

### Paso 5: Restaurar Paquetes

```bash
dotnet restore
```

## ?? Actualizar Versión

Para publicar una nueva versión:

1. Actualiza la propiedad `<Version>` en `ConsoleCLILibrary2.csproj`
2. Ejecuta nuevamente `dotnet pack`
3. El nuevo paquete se generará en `nupkg/`

## ?? Contenido de la Biblioteca

### IDeployService

Interfaz para servicios de despliegue.

```csharp
public interface IDeployService
{
    (string Message, ConsoleColor Color) Deploy(string targetType);
}
```

### DeployService

Implementación del servicio de despliegue que soporta:
- Despliegue en contenedor/container
- Despliegue local

```csharp
var deployService = new DeployService();
var (message, color) = deployService.Deploy("local");
Console.ForegroundColor = color;
Console.WriteLine(message);
Console.ResetColor();
```

## ?? Estructura del Proyecto

```
ConsoleCLILibrary2/
??? ConsoleCLILibrary2.csproj
??? README.md
??? Interfaces/
?   ??? IDeployService.cs
??? Implementations/
    ??? DeployService.cs
```

## ?? Ventajas de la Publicación Local

- ? Desarrollo rápido sin necesidad de publicar en repositorios remotos
- ? Pruebas de integración inmediatas
- ? Sin dependencia de conexión a internet
- ? Control total sobre versiones en desarrollo

## ?? Notas

- El directorio `nupkg/` está (o debería estar) en `.gitignore`
- Los paquetes locales son útiles para desarrollo y pruebas
- Para producción, considera usar GitHub Packages (ver ConsoleCLILibrary)
