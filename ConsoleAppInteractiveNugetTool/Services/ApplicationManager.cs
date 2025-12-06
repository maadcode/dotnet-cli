using ConsoleAppInteractiveNugetTool.Enums;
using ConsoleAppInteractiveNugetTool.Interfaces;
using Spectre.Console;

namespace ConsoleAppInteractiveNugetTool.Services;

public class ApplicationManager : IApplicationManager
{
    private readonly IEnvironmentService _environmentService;
    private readonly Dictionary<AppType, ApplicationInfo> _applications;

    public ApplicationManager(IEnvironmentService environmentService)
    {
        _environmentService = environmentService;
        _applications = InitializeApplications();
    }

    private Dictionary<AppType, ApplicationInfo> InitializeApplications()
    {
        return new Dictionary<AppType, ApplicationInfo>
        {
            { 
                AppType.WebApi, 
                new ApplicationInfo("API REST", "v2.1.0", "Backend API principal", "??") 
            },
            { 
                AppType.MobileApp, 
                new ApplicationInfo("Mobile App", "v1.5.3", "Aplicación móvil iOS/Android", "??") 
            },
            { 
                AppType.DesktopApp, 
                new ApplicationInfo("Desktop Client", "v3.0.1", "Cliente de escritorio Windows", "???") 
            }
        };
    }

    public void DeployApplication(AppType appType, DeploymentEnvironment environment)
    {
        var app = _applications[appType];
        
        AnsiConsole.Clear();
        AnsiConsole.Write(new Rule($"[yellow]Desplegando {app.Name}[/]").RuleStyle("grey").LeftJustified());
        AnsiConsole.WriteLine();

        AnsiConsole.Status()
            .Spinner(Spinner.Known.Star)
            .SpinnerStyle(Style.Parse("green bold"))
            .Start($"[yellow]Preparando despliegue...[/]", ctx =>
            {
                var steps = new[]
                {
                    ("Validando configuración", 800),
                    ("Compilando código", 1200),
                    ("Ejecutando pruebas unitarias", 1500),
                    ("Creando artefactos", 1000),
                    ("Subiendo archivos", 1300),
                    ("Aplicando migraciones", 900),
                    ("Reiniciando servicios", 700),
                    ("Verificando salud de la aplicación", 600)
                };

                foreach (var (step, delay) in steps)
                {
                    ctx.Status($"[yellow]{step}...[/]");
                    Thread.Sleep(delay);
                    AnsiConsole.MarkupLine($"[green]?[/] {step}");
                }
            });

        AnsiConsole.WriteLine();
        
        var successPanel = new Panel(
            $"[green]? Despliegue exitoso[/]\n\n" +
            $"[cyan]Aplicación:[/] {app.Icon} {app.Name}\n" +
            $"[cyan]Versión:[/] {app.Version}\n" +
            $"[cyan]Entorno:[/] [{_environmentService.GetEnvironmentColor()}]{environment}[/]\n" +
            $"[cyan]Fecha:[/] {DateTime.Now:yyyy-MM-dd HH:mm:ss}")
        {
            Border = BoxBorder.Double,
            Header = new PanelHeader("[green bold]?? Resultado del Despliegue[/]")
        }.BorderStyle(Style.Parse("green"));

        AnsiConsole.Write(successPanel);
    }

    public void ShowApplicationStatus(AppType appType)
    {
        var app = _applications[appType];
        
        var tree = new Tree($"[yellow]{app.Icon} {app.Name}[/]")
            .Style("cyan");

        var infoNode = tree.AddNode("[blue]Información General[/]");
        infoNode.AddNode($"Versión: [green]{app.Version}[/]");
        infoNode.AddNode($"Descripción: [dim]{app.Description}[/]");
        infoNode.AddNode($"Estado: [green]? Online[/]");

        var envNode = tree.AddNode("[blue]Entornos Disponibles[/]");
        foreach (DeploymentEnvironment env in Enum.GetValues<DeploymentEnvironment>())
        {
            var status = Random.Shared.Next(0, 10) > 2 ? "[green]? Activo[/]" : "[red]? Inactivo[/]";
            envNode.AddNode($"[{GetEnvColor(env)}]{env}[/]: {status}");
        }

        var metricsNode = tree.AddNode("[blue]Métricas[/]");
        metricsNode.AddNode($"CPU: [green]{Random.Shared.Next(10, 60)}%[/]");
        metricsNode.AddNode($"Memoria: [yellow]{Random.Shared.Next(40, 80)}%[/]");
        metricsNode.AddNode($"Peticiones/seg: [cyan]{Random.Shared.Next(100, 500)}[/]");

        AnsiConsole.Write(tree);
    }

    public List<string> GetAvailableApplications()
    {
        return _applications.Values.Select(app => $"{app.Icon} {app.Name}").ToList();
    }

    private string GetEnvColor(DeploymentEnvironment env)
    {
        return env switch
        {
            DeploymentEnvironment.Development => "blue",
            DeploymentEnvironment.Staging => "yellow",
            DeploymentEnvironment.Production => "red",
            DeploymentEnvironment.Testing => "green",
            DeploymentEnvironment.PreProduction => "orange1",
            _ => "white"
        };
    }

    private class ApplicationInfo
    {
        public string Name { get; }
        public string Version { get; }
        public string Description { get; }
        public string Icon { get; }

        public ApplicationInfo(string name, string version, string description, string icon)
        {
            Name = name;
            Version = version;
            Description = description;
            Icon = icon;
        }
    }
}
