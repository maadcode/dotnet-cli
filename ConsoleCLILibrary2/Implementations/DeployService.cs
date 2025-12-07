using ConsoleCLILibrary2.Interfaces;

namespace ConsoleCLILibrary2.Implementations;

public class DeployService : IDeployService
{
    public (string Message, ConsoleColor Color) Deploy(string targetType)
    {
        return targetType.Trim().ToLower() switch
        {
            "contenedor" or "container" => ("¡Desplegando en contenedor! ??", ConsoleColor.Cyan),
            "local" => ("¡Desplegando en local! ??", ConsoleColor.Green),
            _ => ($"Tipo de despliegue desconocido: {targetType}", ConsoleColor.Red)
        };
    }
}
