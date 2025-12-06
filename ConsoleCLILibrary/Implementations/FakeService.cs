using ConsoleCLILibrary.Interfaces;

namespace ConsoleCLILibrary.Implementations;

public class DevelopmentFakeService : IFakeService
{
    public (string Message, ConsoleColor Color) GetEnvironmentInfo()
        => ("¡Estás en el entorno de Desarrollo!", ConsoleColor.Green);
}

public class StagingFakeService : IFakeService
{
    public (string Message, ConsoleColor Color) GetEnvironmentInfo()
        => ("¡Estás en el entorno de Staging!", ConsoleColor.Yellow);
}

public class ProductionFakeService : IFakeService
{
    public (string Message, ConsoleColor Color) GetEnvironmentInfo()
        => ("¡Cuidado! Estás en Producción.", ConsoleColor.Red);
}

public class UnknownFakeService : IFakeService
{
    private readonly string _environment;

    public UnknownFakeService(string environment)
    {
        _environment = environment;
    }

    public (string Message, ConsoleColor Color) GetEnvironmentInfo()
        => ($"Entorno desconocido: {_environment}", ConsoleColor.Gray);
}