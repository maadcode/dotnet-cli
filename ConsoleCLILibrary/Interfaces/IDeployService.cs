namespace ConsoleCLILibrary.Interfaces;

public interface IDeployService
{
    (string Message, ConsoleColor Color) Deploy(string targetType);
}
