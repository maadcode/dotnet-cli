namespace ConsoleCLILibrary3.Interfaces;

public interface IMigrationService
{
    string ShowVersion();
    (string Message, ConsoleColor Color) ExecuteMigration(string environment);
}
