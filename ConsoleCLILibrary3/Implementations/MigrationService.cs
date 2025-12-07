using ConsoleCLILibrary3.Interfaces;

namespace ConsoleCLILibrary3.Implementations;

public class MigrationService : IMigrationService
{
    private const string Version = "1.0.0";
    
    public string ShowVersion()
    {
        return $"MigrationService v{Version}";
    }
    
    public (string Message, ConsoleColor Color) ExecuteMigration(string environment)
    {
        return environment.Trim().ToLower() switch
        {
            "dev" or "development" => ("? Migraciones ejecutadas en DEV correctamente! ??", ConsoleColor.Green),
            "staging" or "stg" => ("? Migraciones ejecutadas en STAGING correctamente! ??", ConsoleColor.Yellow),
            "prod" or "production" => ("? Migraciones ejecutadas en PRODUCTION correctamente! ??", ConsoleColor.Cyan),
            _ => ($"? Entorno desconocido: {environment}", ConsoleColor.Red)
        };
    }
}
