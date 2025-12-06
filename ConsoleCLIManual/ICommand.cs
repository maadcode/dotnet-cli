namespace ConsoleCLIManual;

/// <summary>
/// Interfaz que deben implementar todos los comandos de la CLI
/// Garantiza que cada comando tenga nombre, descripción, ayuda y ejecución
/// </summary>
public interface ICommand
{
    /// <summary>
    /// Nombre del comando (ej: "onboarding", "deploy")
    /// </summary>
    string Name { get; }
    
    /// <summary>
    /// Descripción breve del comando
    /// </summary>
    string Description { get; }
    
    /// <summary>
    /// Muestra la ayuda detallada del comando
    /// </summary>
    void ShowHelp();
    
    /// <summary>
    /// Ejecuta el comando con los argumentos proporcionados
    /// </summary>
    /// <param name="args">Argumentos de línea de comandos (sin el nombre del comando)</param>
    void Execute(string[] args);
}
