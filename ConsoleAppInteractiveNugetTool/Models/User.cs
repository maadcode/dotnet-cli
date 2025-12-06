using ConsoleAppInteractiveNugetTool.Enums;

namespace ConsoleAppInteractiveNugetTool.Models;

public class User
{
    public string Username { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public UserRole Role { get; set; }
    public DateTime LastLogin { get; set; }
    public bool IsActive { get; set; }

    public User(string username, string fullName, UserRole role)
    {
        Username = username;
        FullName = fullName;
        Role = role;
        LastLogin = DateTime.Now;
        IsActive = true;
    }

    public string GetRoleDescription() => Role switch
    {
        UserRole.Admin => "Administrator - Full access to all systems",
        UserRole.Developer => "Developer - Access to development and staging environments",
        UserRole.DevOps => "DevOps Engineer - Access to deployment and infrastructure",
        UserRole.QA => "Quality Assurance - Access to testing environments",
        UserRole.Guest => "Guest - Read-only access",
        _ => "Unknown role"
    };
}
