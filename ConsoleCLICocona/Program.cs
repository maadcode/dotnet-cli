using Cocona;
using ConsoleCLILibrary.Implementations;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleCLICocona;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = CoconaApp.CreateBuilder();
        
        builder.Services.AddSingleton<DevelopmentFakeService>();
        builder.Services.AddSingleton<StagingFakeService>();
        builder.Services.AddSingleton<ProductionFakeService>();
        builder.Services.AddSingleton<DeployService>();
        
        var app = builder.Build();
        app.AddCommands<ServiceCommands>();
        app.Run();
    }
}

