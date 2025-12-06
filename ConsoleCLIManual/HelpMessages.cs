namespace ConsoleCLIManual;

/// <summary>
/// Contiene los mensajes de ayuda y error para la CLI
/// Usa los comandos registrados para generar la ayuda automáticamente
/// </summary>
public static class HelpMessages
{
    /// <summary>
    /// Muestra el mensaje de error cuando no se encuentra el comando
    /// </summary>
    public static void ShowCommandNotFoundError(IEnumerable<ICommand> commands)
    {
        Console.WriteLine("Usage: dotnet run -- <command> [options]");
        Console.WriteLine();
        Console.WriteLine("Commands:");
        
        foreach (var command in commands)
        {
            Console.WriteLine($"  {command.Name,-15} {command.Description}");
        }
        
        Console.WriteLine();
        Console.WriteLine("Run 'dotnet run -- --help' for more information.");
    }

    /// <summary>
    /// Muestra la ayuda general de la aplicación
    /// </summary>
    public static void ShowGeneralHelp(IEnumerable<ICommand> commands)
    {
        Console.WriteLine("Usage:");
        Console.WriteLine("  dotnet run -- <command> [options]");
        Console.WriteLine();
        Console.WriteLine("Commands:");
        
        foreach (var command in commands)
        {
            Console.WriteLine($"  {command.Name,-15} {command.Description}");
        }
        
        Console.WriteLine();
        Console.WriteLine("Options:");
        Console.WriteLine("  -h, --help    Show help");
        Console.WriteLine();
        Console.WriteLine("Run 'dotnet run -- <command> --help' for more information about a command.");
    }
}


