using ConsoleAppInteractiveNugetTool.Enums;

namespace ConsoleAppInteractiveNugetTool.Interfaces;

public interface IEnvironmentService
{
    DeploymentEnvironment CurrentEnvironment { get; }
    void SetEnvironment(DeploymentEnvironment environment);
    Dictionary<string, string> GetEnvironmentVariables();
    string GetEnvironmentColor();
    void DisplayEnvironmentInfo();
}
