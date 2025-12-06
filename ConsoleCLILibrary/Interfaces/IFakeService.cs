namespace ConsoleCLILibrary.Interfaces;

public interface IFakeService
{
    (string Message, ConsoleColor Color) GetEnvironmentInfo();
}