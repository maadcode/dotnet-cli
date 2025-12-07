using Cocona;
using ConsoleCLILibrary.Implementations;
using ConsoleCLILibrary.Interfaces;
using ConsoleCLILibrary2.Implementations;
using ConsoleCLILibrary3.Interfaces;

namespace ConsoleCLICocona;

public class ServiceCommands
{
    private readonly DevelopmentFakeService _developmentService;
    private readonly StagingFakeService _stagingService;
    private readonly ProductionFakeService _productionService;
    private readonly DeployService _deployService;
    private readonly IMigrationService _migrationService;
    
    // Constructor con inyección de dependencias (Cocona lo hace automáticamente)
    public ServiceCommands(
        DevelopmentFakeService developmentService,
        StagingFakeService stagingService,
        ProductionFakeService productionService,
        DeployService deployService,
        IMigrationService migrationService)
    {
        _developmentService = developmentService;
        _stagingService = stagingService;
        _productionService = productionService;
        _deployService = deployService;
        _migrationService = migrationService;
    }

    // [Command] - Define un comando ejecutable
    // Se ejecuta con: dotnet run -- onboarding -e production
    [Command("onboarding")]
    public void Onboarding(
        // [Option] - Define una opción con:
        // - Short name: 'e' ? se usa como -e
        // - Description: Texto de ayuda generado automáticamente
        // - Valor por defecto: "Development"
        [Option('e', Description = "Entorno a simular: Development, Staging, Production")] 
        string environment = "Development")
    {
        // Seleccionar el servicio según el parámetro environment
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

    // [Command] - Define el comando deploy
    // Se ejecuta con: dotnet run -- deploy -t contenedor
    [Command("deploy")]
    public void Deploy(
        // [Option] - Define una opción con:
        // - Short name: 't' ? se usa como -t
        // - Description: Texto de ayuda generado automáticamente
        // - Valor por defecto: "local"
        [Option('t', Description = "Tipo de despliegue: contenedor o local")] 
        string target = "local")
    {
        var (message, color) = _deployService.Deploy(target);
        var prevColor = Console.ForegroundColor;
        Console.ForegroundColor = color;
        Console.WriteLine(message);
        Console.ForegroundColor = prevColor;
    }

    // [Command] - Define el comando migration
    // Se ejecuta con: dotnet run -- migration -e dev
    [Command("migration")]
    public void Migration(
        // [Option] - Define una opción con:
        // - Short name: 'e' ? se usa como -e
        // - Description: Texto de ayuda generado automáticamente
        // - Valor por defecto: "dev"
        [Option('e', Description = "Entorno de migración: dev, staging, prod")] 
        string environment = "dev")
    {
        Console.WriteLine($"Version: {_migrationService.ShowVersion()}");
        Console.WriteLine();

        var result = _migrationService.ExecuteMigration(environment);
        var prevColor = Console.ForegroundColor;
        Console.ForegroundColor = result.Color;
        Console.WriteLine(result.Message);
        Console.ForegroundColor = prevColor;
    }
}

