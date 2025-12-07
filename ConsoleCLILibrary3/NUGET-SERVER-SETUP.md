# NuGet Server Local con BaGet

## ¿Qué es BaGet?

BaGet es un servidor NuGet moderno, ligero y open-source compatible con .NET. Es perfecto para alojar paquetes NuGet de forma local o privada.

- **Repositorio:** https://github.com/loic-sharma/BaGet
- **Documentación:** https://loic-sharma.github.io/BaGet/

## Características

? Open Source (Licencia MIT)  
? Compatible con Docker/Podman  
? Interfaz web incluida  
? Compatible con el protocolo NuGet v3  
? Búsqueda y exploración de paquetes  
? Compatible con Symbol packages  

---

## Opción 1: Ejecutar con Podman (Recomendado)

### Iniciar el servidor BaGet

```bash
podman run -d \
  --name baget \
  -p 5555:80 \
  -e ApiKey=NUGET-SERVER-API-KEY \
  loicsharma/baget:latest
```

**Nota:** Usamos el puerto 5555 para evitar el error `ERR_UNSAFE_PORT` que ocurre con el puerto 6000 en navegadores Chrome/Edge.

### Verificar que está corriendo

```bash
podman ps
```

### Ver logs (importante para debugging)

```bash
podman logs baget
```

### Acceder a la interfaz web

Abre tu navegador en: http://localhost:5555

### Detener el servidor

```bash
podman stop baget
```

### Iniciar el servidor nuevamente

```bash
podman start baget
```

### Eliminar el contenedor

```bash
podman rm -f baget
```

---

## Opción 2: Ejecutar con Docker

Si prefieres Docker en lugar de Podman:

```bash
docker run -d \
  --name baget \
  -p 5555:80 \
  -e ApiKey=NUGET-SERVER-API-KEY \
  loicsharma/baget:latest
```

---

## Opción 3: Clonar y ejecutar desde el código fuente

### Clonar el repositorio

```bash
git clone https://github.com/loic-sharma/BaGet.git
cd BaGet
```

### Ejecutar con .NET

```bash
cd src/BaGet
dotnet run
```

Por defecto se ejecutará en: http://localhost:5000

---

## Configuración de NuGet para usar el servidor local

### ? Opción 1: Usar el NuGet.config de la solución (RECOMENDADO)

La solución ya incluye el archivo `NuGet.config` configurado con BaGet local:

```xml
<packageSources>
  <add key="baget-local" value="http://localhost:5555/v3/index.json" />
</packageSources>

<packageSourceMapping>
  <packageSource key="baget-local">
    <package pattern="ConsoleCLILibrary3" />
  </packageSource>
</packageSourceMapping>
```

**No necesitas hacer nada adicional** - la configuración ya está lista. ?

### Opción 2: Configuración global (alternativa)

Si prefieres agregar BaGet globalmente en tu máquina:

```bash
dotnet nuget add source http://localhost:5555/v3/index.json --name BaGetLocal
```

### Listar fuentes configuradas

```bash
dotnet nuget list source
```

### Eliminar fuente (si es necesario)

```bash
dotnet nuget remove source BaGetLocal
```

---

## Publicar ConsoleCLILibrary3 en BaGet

### 1. Empaquetar la biblioteca

```bash
dotnet pack ConsoleCLILibrary3/ConsoleCLILibrary3.csproj -c Release -o ./nupkg
```

### 2. Publicar en BaGet

```bash
dotnet nuget push ./nupkg/ConsoleCLILibrary3.1.0.0.nupkg --source http://localhost:5555/v3/index.json --api-key NUGET-SERVER-API-KEY
```

**Nota:** Por defecto, BaGet acepta cualquier API key. Puedes usar cualquier valor, por ejemplo: `NUGET-SERVER-API-KEY`

### 3. Verificar en la interfaz web

Abre http://localhost:5555 y deberías ver tu paquete listado.

---

## Consumir ConsoleCLILibrary3 desde otro proyecto

### 1. Agregar el paquete

Como el `NuGet.config` ya está configurado, simplemente agrega el paquete:

```bash
dotnet add package ConsoleCLILibrary3
```

El paquete se descargará automáticamente desde BaGet local (http://localhost:5555) gracias al `packageSourceMapping`. ??

### 2. Usar en tu código

```csharp
using ConsoleCLILibrary3.Interfaces;
using ConsoleCLILibrary3.Implementations;

var migrationService = new MigrationService();
Console.WriteLine(migrationService.ShowVersion());

var result = migrationService.ExecuteMigration("dev");
Console.ForegroundColor = result.Color;
Console.WriteLine(result.Message);
Console.ResetColor();
```

---

## Arquitectura de fuentes NuGet en la solución

```
NuGet.config
??? ?? local (./nupkg)
?   ??? ConsoleCLILibrary2
??? ?? github (GitHub Packages)
?   ??? ConsoleCLILibrary
??? ?? baget-local (http://localhost:5555)
?   ??? ConsoleCLILibrary3
??? ?? nuget.org (NuGet oficial)
    ??? Todos los demás paquetes (*, Cocona, etc.)
```

---

## Comandos útiles de Podman/Docker

```bash
podman logs baget
# o
docker logs baget
```

### Ver contenedores en ejecución

```bash
podman ps
# o
docker ps
```

### Inspeccionar el contenedor

```bash
podman inspect baget
# o
docker inspect baget
```

---

## Troubleshooting

### El puerto 6000 da error ERR_UNSAFE_PORT

Chrome y Edge bloquean ciertos puertos por razones de seguridad (6000, 6665-6669, etc.). 

**Solución:** Usa el puerto 5555 como se indica en esta documentación.

### El puerto 5555 ya está en uso

Cambia el puerto del host:

```bash
podman run -d --name baget -p 7777:80 -e ApiKey=NUGET-SERVER-API-KEY loicsharma/baget:latest
```

Luego actualiza el `NuGet.config`:
```xml
<add key="baget-local" value="http://localhost:7777/v3/index.json" />
```

### No puedo publicar paquetes

Verifica que el servidor esté corriendo:

```bash
curl http://localhost:5555/v3/index.json
```

Deberías recibir una respuesta JSON con los endpoints del servidor.

### El contenedor no inicia o muestra errores

Ver los logs del contenedor:

```bash
podman logs baget
```

Si ves errores de permisos o configuración, intenta:

```bash
# Eliminar el contenedor anterior
podman rm -f baget

# Crear uno nuevo con la configuración correcta
podman run -d \
  --name baget \
  -p 5555:80 \
  -e ApiKey=NUGET-SERVER-API-KEY \
  loicsharma/baget:latest
```

---

## Resumen de comandos rápidos

```bash
# 1. Iniciar BaGet (IMPORTANTE: usar puerto 5555, no 6000)
podman run -d --name baget -p 5555:80 -e ApiKey=NUGET-SERVER-API-KEY loicsharma/baget:latest

# 2. Verificar que está corriendo
podman logs baget

# 3. Empaquetar biblioteca
dotnet pack ConsoleCLILibrary3/ConsoleCLILibrary3.csproj -c Release -o ./nupkg

# 4. Publicar en BaGet
dotnet nuget push ./nupkg/ConsoleCLILibrary3.1.0.0.nupkg --source http://localhost:5555/v3/index.json --api-key NUGET-SERVER-API-KEY

# 5. Ver interfaz web
# Abre: http://localhost:5555

# 6. Usar el paquete (el NuGet.config ya está configurado)
dotnet add package ConsoleCLILibrary3
```

---

## Próximos pasos

1. ? Crear ConsoleCLILibrary3 con IMigrationService y MigrationService
2. ? Configurar NuGet.config con BaGet local
3. ? Levantar BaGet con Podman (puerto 5555)
4. ? Empaquetar ConsoleCLILibrary3
5. ? Publicar en BaGet local
6. ? Consumir el paquete desde otro proyecto de la solución

---

**¡Listo para usar tu propio NuGet Server local!** ??
