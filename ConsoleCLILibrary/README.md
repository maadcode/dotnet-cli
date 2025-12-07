# ConsoleCLILibrary

Biblioteca de .NET para aplicaciones CLI con servicios de utilidad (FakeService).

## ?? Publicación en GitHub Packages

Este proyecto está configurado para publicar paquetes NuGet en **GitHub Packages**.

### Requisitos Previos

- .NET SDK 10.0 o superior
- Cuenta de GitHub con acceso al repositorio
- Personal Access Token (PAT) de GitHub con permisos `write:packages` y `read:packages`

### Paso 1: Crear un Personal Access Token (PAT)

1. Ve a **GitHub** ? **Settings** ? **Developer settings** ? **Personal access tokens** ? **Tokens (classic)**
2. Haz clic en **Generate new token (classic)**
3. Dale un nombre descriptivo (ej: "NuGet Package Publishing")
4. Selecciona los siguientes scopes:
   - ? `write:packages` (para publicar paquetes)
   - ? `read:packages` (para descargar paquetes)
   - ? `delete:packages` (opcional, para eliminar paquetes)
5. Genera el token y **cópialo** (no podrás volver a verlo)

### Paso 2: Configurar las Credenciales de NuGet

Guarda tu token como variable de entorno o configúralo directamente:

#### Opción A: Variable de Entorno (Recomendado)

```bash
# Windows (PowerShell)
$env:GITHUB_TOKEN = "tu_personal_access_token"

# Linux/Mac
export GITHUB_TOKEN="tu_personal_access_token"
```

#### Opción B: Configurar en NuGet

```bash
dotnet nuget add source "https://nuget.pkg.github.com/maadcode/index.json" \
  --name github \
  --username tu_usuario_github \
  --password tu_personal_access_token \
  --store-password-in-clear-text
```

### Paso 3: Generar el Paquete NuGet

```bash
dotnet pack ConsoleCLILibrary/ConsoleCLILibrary.csproj --configuration Release
```

Esto generará el archivo `.nupkg` en el directorio `nupkg/`.

### Paso 4: Publicar en GitHub Packages

```bash
dotnet nuget push "nupkg/ConsoleCLILibrary.1.0.0.nupkg" \
  --source "github" \
  --api-key $env:GITHUB_TOKEN
```

O especificando la URL completa:

```bash
dotnet nuget push "nupkg/ConsoleCLILibrary.1.0.0.nupkg" \
  --source "https://nuget.pkg.github.com/maadcode/index.json" \
  --api-key $env:GITHUB_TOKEN
```

### Paso 5: Verificar la Publicación

1. Ve a tu repositorio en GitHub
2. Haz clic en **Packages** (en la barra lateral derecha)
3. Deberías ver tu paquete **ConsoleCLILibrary** listado

### Paso 6: Consumir el Paquete desde Otro Proyecto

#### Configurar NuGet.config

Asegúrate de que tu proyecto tenga un archivo `NuGet.config` con la fuente de GitHub:

```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <packageSources>
    <add key="github" value="https://nuget.pkg.github.com/maadcode/index.json" />
    <add key="nuget.org" value="https://api.nuget.org/v3/index.json" protocolVersion="3" />
  </packageSources>
  <packageSourceCredentials>
    <github>
      <add key="Username" value="tu_usuario_github" />
      <add key="ClearTextPassword" value="tu_personal_access_token" />
    </github>
  </packageSourceCredentials>
</configuration>
```

#### Instalar el Paquete

```bash
dotnet add package ConsoleCLILibrary --version 1.0.0
dotnet restore
```

## ?? Actualizar Versión

Para publicar una nueva versión:

1. Actualiza la propiedad `<Version>` en `ConsoleCLILibrary.csproj`
2. Ejecuta `dotnet pack`
3. Ejecuta `dotnet nuget push` con el nuevo archivo `.nupkg`

## ?? Contenido de la Biblioteca

### IFakeService

Interfaz para servicios de prueba.

```csharp
public interface IFakeService
{
    string GetMessage();
}
```

### FakeService

Implementación de ejemplo del servicio.

```csharp
var fakeService = new FakeService();
Console.WriteLine(fakeService.GetMessage());
```

## ?? Estructura del Proyecto

```
ConsoleCLILibrary/
??? ConsoleCLILibrary.csproj
??? README.md
??? Interfaces/
?   ??? IFakeService.cs
??? Implementations/
    ??? FakeService.cs
```

## ?? Ventajas de GitHub Packages

- ? Integración nativa con GitHub
- ? Control de acceso basado en permisos del repositorio
- ? Versionado automático
- ? Ideal para paquetes compartidos en equipos
- ? Visible en el repositorio de GitHub

## ?? Notas Importantes

- Los paquetes en GitHub Packages son **privados por defecto** si el repositorio es privado
- Necesitas autenticación incluso para descargar paquetes privados
- El token PAT debe mantenerse **seguro** y **nunca** commitear al repositorio
- Para desarrollo local y pruebas rápidas, considera usar **ConsoleCLILibrary2** (publicación local)

## ?? Enlaces Útiles

- [Documentación GitHub Packages](https://docs.github.com/en/packages)
- [Trabajar con el registro de NuGet](https://docs.github.com/en/packages/working-with-a-github-packages-registry/working-with-the-nuget-registry)
