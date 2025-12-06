using ConsoleAppInteractiveNugetTool.Enums;

namespace ConsoleAppInteractiveNugetTool.Interfaces;

public interface IApplicationManager
{
    void DeployApplication(AppType appType, DeploymentEnvironment environment);
    void ShowApplicationStatus(AppType appType);
    List<string> GetAvailableApplications();
}
