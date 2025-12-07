using Cocona;
using ConsoleCLILibrary.Implementations;
using ConsoleCLILibrary.Interfaces;
using ConsoleCLILibrary2.Implementations;

namespace ConsoleCLICocona;

public class ServiceCommands
{
    private readonly DevelopmentFakeService _developmentService;
    private readonly StagingFakeService _stagingService;
    private readonly ProductionFakeService _productionService;
    private readonly DeployService _deployService;
    
    // Constructor con inyección de dependencias (Cocona lo hace automáticamente)
    public ServiceCommands(
        DevelopmentFakeService developmentService,
        StagingFakeService stagingService,
        ProductionFakeService productionService,
        DeployService deployService)
    {
        _developmentService = developmentService;
        _stagingService = stagingService;
        _productionService = productionService;
        _deployService = deployService;
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
}

