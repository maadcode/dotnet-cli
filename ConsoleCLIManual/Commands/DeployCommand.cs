using ConsoleCLILibrary2.Implementations;

namespace ConsoleCLIManual.Commands;

/// <summary>
/// Comando de deploy que despliega en contenedor o local
/// </summary>
public class DeployCommand : ICommand
{
    private readonly DeployService _deployService;

    public string Name => "deploy";
    public string Description => "Despliega la aplicación en contenedor o local";

    public DeployCommand(DeployService deployService)
    {
        _deployService = deployService;
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
        Console.WriteLine("  -t, --target <target>    Tipo de despliegue: contenedor o local (default: local)");
        Console.WriteLine("  -h, --help               Show help");
        Console.WriteLine();
        Console.WriteLine("Examples:");
        Console.WriteLine($"  dotnet run -- {Name}");
        Console.WriteLine($"  dotnet run -- {Name} -t contenedor");
        Console.WriteLine($"  dotnet run -- {Name} --target local");
    }

    public void Execute(string[] args)
    {
        var target = ParseTarget(args);

        var (message, color) = _deployService.Deploy(target);
        var prevColor = Console.ForegroundColor;
        Console.ForegroundColor = color;
        Console.WriteLine(message);
        Console.ForegroundColor = prevColor;
    }

    private static string ParseTarget(string[] args)
    {
        var target = "local";

        for (int i = 0; i < args.Length; i++)
        {
            if (args[i] == "--target" || args[i] == "-t")
            {
                if (i + 1 < args.Length)
                {
                    target = args[i + 1].Trim().ToLower();
                    i++;
                }
            }
        }

        return target;
    }
}
