using ConsoleCLILibrary.Implementations;
using ConsoleCLILibrary.Interfaces;

namespace ConsoleCLIManual.Commands;

/// <summary>
/// Comando de onboarding que simula diferentes entornos
/// </summary>
public class OnboardingCommand : ICommand
{
    private readonly DevelopmentFakeService _developmentService;
    private readonly StagingFakeService _stagingService;
    private readonly ProductionFakeService _productionService;

    public string Name => "onboarding";
    public string Description => "Ejecuta el proceso de onboarding";

    public OnboardingCommand(
        DevelopmentFakeService developmentService,
        StagingFakeService stagingService,
        ProductionFakeService productionService)
    {
        _developmentService = developmentService;
        _stagingService = stagingService;
        _productionService = productionService;
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
        Console.WriteLine("  -e, --environment <environment>    Entorno a simular: Development, Staging, Production (default: Development)");
        Console.WriteLine("  -h, --help                         Show help");
        Console.WriteLine();
        Console.WriteLine("Examples:");
        Console.WriteLine($"  dotnet run -- {Name}");
        Console.WriteLine($"  dotnet run -- {Name} -e production");
        Console.WriteLine($"  dotnet run -- {Name} --environment staging");
    }

    public void Execute(string[] args)
    {
        var environment = ParseEnvironment(args);

        IFakeService service = environment.Trim().ToLower() switch
        {
            "development" => _developmentService,
            "staging" => _stagingService,
            "production" => _productionService,
            _ => new UnknownFakeService(environment)
        };

        var (message, color) = service.GetEnvironmentInfo();
        var prevColor = Console.ForegroundColor;
        Console.ForegroundColor = color;
        Console.WriteLine(message);
        Console.ForegroundColor = prevColor;
    }

    private static string ParseEnvironment(string[] args)
    {
        var environment = "development";

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
