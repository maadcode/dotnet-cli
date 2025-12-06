using ConsoleAppInteractiveNugetTool.Enums;
using ConsoleAppInteractiveNugetTool.Interfaces;
using ConsoleAppInteractiveNugetTool.Models;
using Spectre.Console;

namespace ConsoleAppInteractiveNugetTool.Services;

public class AuthenticationService : IAuthenticationService
{
    private User? _currentUser;
    private readonly Dictionary<string, (string Password, UserRole Role, string FullName)> _users;

    public AuthenticationService()
    {
        _users = new Dictionary<string, (string, UserRole, string)>
        {
            { "admin", ("admin123", UserRole.Admin, "Administrator User") },
            { "dev", ("dev123", UserRole.Developer, "John Developer") },
            { "devops", ("devops123", UserRole.DevOps, "Jane DevOps") },
            { "qa", ("qa123", UserRole.QA, "Bob Tester") },
            { "guest", ("guest123", UserRole.Guest, "Guest User") }
        };
    }

    public async Task<User?> AuthenticateAsync(string username, string password)
    {
        AnsiConsole.Status()
            .Spinner(Spinner.Known.Dots)
            .Start("[yellow]Autenticando...[/]", ctx =>
            {
                Thread.Sleep(1500);
                ctx.Status("[green]Verificando credenciales...[/]");
                Thread.Sleep(500);
            });

        if (ValidateCredentials(username, password))
        {
            var userInfo = _users[username.ToLower()];
            _currentUser = new User(username, userInfo.FullName, userInfo.Role);
            
            AnsiConsole.MarkupLine("[green]? Autenticación exitosa![/]");
            await Task.Delay(500);
            
            return _currentUser;
        }

        AnsiConsole.MarkupLine("[red]? Credenciales inválidas[/]");
        await Task.Delay(1000);
        return null;
    }

    public bool ValidateCredentials(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            return false;

        var usernameLower = username.ToLower();
        return _users.ContainsKey(usernameLower) && _users[usernameLower].Password == password;
    }

    public User? GetCurrentUser()
    {
        return _currentUser;
    }

    public void Logout()
    {
        _currentUser = null;
        AnsiConsole.MarkupLine("[yellow]Sesión cerrada correctamente[/]");
    }
}
