using ConsoleAppInteractiveNugetTool.Models;

namespace ConsoleAppInteractiveNugetTool.Interfaces;

public interface IAuthenticationService
{
    Task<User?> AuthenticateAsync(string username, string password);
    bool ValidateCredentials(string username, string password);
    User? GetCurrentUser();
    void Logout();
}
