namespace ConsoleCLIManual;

/// <summary>
/// Orquestador principal de comandos
/// Descubre y ejecuta comandos que implementan ICommand automáticamente
/// </summary>
public class ServiceCommands
{
    private readonly IEnumerable<ICommand> _commands;

    public ServiceCommands(IEnumerable<ICommand> commands)
    {
        _commands = commands;
    }

    /// <summary>
    /// Punto de entrada principal para ejecutar comandos
    /// Descubre comandos registrados y ejecuta el solicitado
    /// </summary>
    public void Start(string[] args)
    {
        // Validación de ayuda general (solo si no hay argumentos o el primer argumento es --help)
        if (args.Length < 1)
        {
            HelpMessages.ShowGeneralHelp(_commands);
            return;
        }

        // Si el primer argumento es --help o -h, mostrar ayuda general
        if (args[0] == "--help" || args[0] == "-h")
        {
            HelpMessages.ShowGeneralHelp(_commands);
            return;
        }

        // Obtener el comando solicitado
        var commandName = args[0].ToLower();

        // Buscar el comando en los registrados
        var command = _commands.FirstOrDefault(c => c.Name.Equals(commandName, StringComparison.OrdinalIgnoreCase));

        if (command == null)
        {
            HelpMessages.ShowCommandNotFoundError(_commands);
            return;
        }

        // Validación de ayuda del comando específico
        if (args.Skip(1).Contains("--help") || args.Skip(1).Contains("-h"))
        {
            command.ShowHelp();
            return;
        }

        // Ejecutar el comando (sin el nombre del comando en los args)
        command.Execute(args.Skip(1).ToArray());
    }
}


