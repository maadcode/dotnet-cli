using ConsoleAppInteractiveNugetTool.Enums;
using ConsoleAppInteractiveNugetTool.Interfaces;
using Spectre.Console;

namespace ConsoleAppInteractiveNugetTool.Services;

public class EnvironmentService : IEnvironmentService
{
    public DeploymentEnvironment CurrentEnvironment { get; private set; }

    public EnvironmentService()
    {
        CurrentEnvironment = DeploymentEnvironment.Development;
    }

    public void SetEnvironment(DeploymentEnvironment environment)
    {
        var oldEnvironment = CurrentEnvironment;
        CurrentEnvironment = environment;

        var panel = new Panel($"[{GetEnvironmentColor()}]Entorno cambiado:[/]\n" +
                             $"[dim]{oldEnvironment}[/] ? [{GetEnvironmentColor()}]{environment}[/]")
        {
            Border = BoxBorder.Rounded,
            Padding = new Padding(2, 1),
            Header = new PanelHeader($"[{GetEnvironmentColor()}]? Configuración de Entorno[/]")
        };

        AnsiConsole.Write(panel);
    }

    public Dictionary<string, string> GetEnvironmentVariables()
    {
        return CurrentEnvironment switch
        {
            DeploymentEnvironment.Development => new Dictionary<string, string>
            {
                { "API_URL", "http://localhost:5000" },
                { "DB_CONNECTION", "Server=localhost;Database=DevDB;" },
                { "LOG_LEVEL", "Debug" },
                { "CACHE_ENABLED", "false" }
            },
            DeploymentEnvironment.Staging => new Dictionary<string, string>
            {
                { "API_URL", "https://staging-api.company.com" },
                { "DB_CONNECTION", "Server=staging-db.company.com;Database=StagingDB;" },
                { "LOG_LEVEL", "Information" },
                { "CACHE_ENABLED", "true" }
            },
            DeploymentEnvironment.Production => new Dictionary<string, string>
            {
                { "API_URL", "https://api.company.com" },
                { "DB_CONNECTION", "Server=prod-db.company.com;Database=ProductionDB;" },
                { "LOG_LEVEL", "Warning" },
                { "CACHE_ENABLED", "true" }
            },
            DeploymentEnvironment.Testing => new Dictionary<string, string>
            {
                { "API_URL", "http://test-api.company.local" },
                { "DB_CONNECTION", "Server=test-db;Database=TestDB;" },
                { "LOG_LEVEL", "Debug" },
                { "CACHE_ENABLED", "false" }
            },
            DeploymentEnvironment.PreProduction => new Dictionary<string, string>
            {
                { "API_URL", "https://preprod-api.company.com" },
                { "DB_CONNECTION", "Server=preprod-db.company.com;Database=PreProdDB;" },
                { "LOG_LEVEL", "Information" },
                { "CACHE_ENABLED", "true" }
            },
            _ => new Dictionary<string, string>()
        };
    }

    public string GetEnvironmentColor()
    {
        return CurrentEnvironment switch
        {
            DeploymentEnvironment.Development => "blue",
            DeploymentEnvironment.Staging => "yellow",
            DeploymentEnvironment.Production => "red",
            DeploymentEnvironment.Testing => "green",
            DeploymentEnvironment.PreProduction => "orange1",
            _ => "white"
        };
    }

    public void DisplayEnvironmentInfo()
    {
        var color = GetEnvironmentColor();
        
        var table = new Table()
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Grey)
            .AddColumn(new TableColumn("[bold]Variable[/]").Centered())
            .AddColumn(new TableColumn("[bold]Valor[/]").Centered());

        table.Title = new TableTitle($"[{color}]?? Entorno: {CurrentEnvironment}[/]");

        foreach (var variable in GetEnvironmentVariables())
        {
            table.AddRow($"[cyan]{variable.Key}[/]", $"[dim]{variable.Value}[/]");
        }

        AnsiConsole.Write(table);
    }
}
