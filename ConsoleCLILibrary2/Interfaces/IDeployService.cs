namespace ConsoleCLILibrary2.Interfaces;

public interface IDeployService
{
    (string Message, ConsoleColor Color) Deploy(string targetType);
}
