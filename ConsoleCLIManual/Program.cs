using ConsoleCLILibrary.Implementations;
using ConsoleCLIManual.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleCLIManual;

public class Program
{
    public static void Main(string[] args)
    {
        var services = new ServiceCollection();
        
        // Registrar servicios de negocio
        services.AddSingleton<DevelopmentFakeService>();
        services.AddSingleton<StagingFakeService>();
        services.AddSingleton<ProductionFakeService>();
        services.AddSingleton<DeployService>();
        
        // Registrar comandos (cada comando implementa ICommand)
        services.AddTransient<ICommand, OnboardingCommand>();
        services.AddTransient<ICommand, DeployCommand>();
        
        // Registrar orquestador de comandos
        services.AddTransient<ServiceCommands>();

        var serviceProvider = services.BuildServiceProvider();
        var command = serviceProvider.GetRequiredService<ServiceCommands>();
        
        command.Start(args);
    }
}

