using ConsoleCLILibrary3.Interfaces;

namespace ConsoleCLIManual.Commands;

/// <summary>
/// Comando de migration que ejecuta migraciones de base de datos
/// </summary>
public class MigrationCommand : ICommand
{
    private readonly IMigrationService _migrationService;

    public string Name => "migration";
    public string Description => "Ejecuta migraciones de base de datos en diferentes entornos";

    public MigrationCommand(IMigrationService migrationService)
    {
        _migrationService = migrationService;
    }

    public void ShowHelp()
    {
        Console.WriteLine("Description:");
        Console.WriteLine($"  {Description}");
        Console.WriteLine();
        Console.WriteLine("Usage:");
        Console.WriteLine($"  dotnet run -- {Name} [options]");
        Console.WriteLine();
        Console.WriteLine("Options:");
        Console.WriteLine("  -e, --environment <env>    Entorno de migraci?n: dev, staging, prod (default: dev)");
        Console.WriteLine("  -h, --help                 Show help");
        Console.WriteLine();
        Console.WriteLine("Examples:");
        Console.WriteLine($"  dotnet run -- {Name}");
        Console.WriteLine($"  dotnet run -- {Name} -e dev");
        Console.WriteLine($"  dotnet run -- {Name} --environment staging");
        Console.WriteLine($"  dotnet run -- {Name} -e prod");
    }

    public void Execute(string[] args)
    {
        Console.WriteLine($"Version: {_migrationService.ShowVersion()}");
        Console.WriteLine();

        var environment = ParseEnvironment(args);
        var result = _migrationService.ExecuteMigration(environment);

        var prevColor = Console.ForegroundColor;
        Console.ForegroundColor = result.Color;
        Console.WriteLine(result.Message);
        Console.ForegroundColor = prevColor;
    }

    private static string ParseEnvironment(string[] args)
    {
        var environment = "dev";

        for (int i = 0; i < args.Length; i++)
        {
            if (args[i] == "--environment" || args[i] == "-e")
            {
                if (i + 1 < args.Length)
                {
                    environment = args[i + 1].Trim().ToLower();
                    i++;
                }
            }
        }

        return environment;
    }
}
