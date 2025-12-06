using ConsoleAppInteractiveNugetTool.Enums;
using ConsoleAppInteractiveNugetTool.Interfaces;
using ConsoleAppInteractiveNugetTool.Services;
using Spectre.Console;

namespace ConsoleAppInteractiveNugetTool;

class Program
{
    private static IAuthenticationService? _authService;
    private static IEnvironmentService? _envService;
    private static IApplicationManager? _appManager;
    private static SpectreShowcase? _showcase;

    static async Task Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        
        _authService = new AuthenticationService();
        _envService = new EnvironmentService();
        _appManager = new ApplicationManager(_envService);
        _showcase = new SpectreShowcase();

        ShowWelcomeScreen();
        
        var user = await LoginScreen();
        
        if (user == null)
        {
            AnsiConsole.MarkupLine("[red]No se pudo autenticar. Saliendo...[/]");
            return;
        }

        ShowUserWelcome();
        
        await MainMenu();
    }

    static void ShowWelcomeScreen()
    {
        AnsiConsole.Clear();
        
        var rule = new Rule("[blue bold]?? Sistema de Gestión de Aplicaciones[/]")
        {
            Style = Style.Parse("blue"),
            Justification = Justify.Center
        };
        
        AnsiConsole.Write(rule);
        AnsiConsole.WriteLine();

        var panel = new Panel(
            "[cyan]Bienvenido al sistema de gestión de aplicaciones empresariales.[/]\n" +
            "[dim]Este sistema permite gestionar despliegues, monitorear aplicaciones\n" +
            "y administrar entornos de desarrollo.[/]")
        {
            Border = BoxBorder.Rounded,
            Padding = new Padding(2, 1)
        }.BorderStyle(Style.Parse("blue"));

        AnsiConsole.Write(panel);
        AnsiConsole.WriteLine();
    }

    static async Task<bool> LoginScreen()
    {
        AnsiConsole.Write(new Rule("[yellow]?? Inicio de Sesión[/]").RuleStyle("grey").LeftJustified());
        AnsiConsole.WriteLine();

        var table = new Table()
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Grey)
            .AddColumn("[bold]Usuario[/]")
            .AddColumn("[bold]Contraseña[/]")
            .AddColumn("[bold]Rol[/]");

        table.AddRow("[cyan]admin[/]", "[dim]admin123[/]", "[green]Admin[/]");
        table.AddRow("[cyan]dev[/]", "[dim]dev123[/]", "[blue]Developer[/]");
        table.AddRow("[cyan]devops[/]", "[dim]devops123[/]", "[yellow]DevOps[/]");
        table.AddRow("[cyan]qa[/]", "[dim]qa123[/]", "[purple]QA[/]");
        table.AddRow("[cyan]guest[/]", "[dim]guest123[/]", "[grey]Guest[/]");

        AnsiConsole.Write(table);
        AnsiConsole.WriteLine();

        int attempts = 0;
        const int maxAttempts = 3;

        while (attempts < maxAttempts)
        {
            var username = AnsiConsole.Ask<string>("Ingrese [cyan]usuario[/]:");
            var password = AnsiConsole.Prompt(
                new TextPrompt<string>("Ingrese [cyan]contraseña[/]:")
                    .PromptStyle("cyan")
                    .Secret());

            var user = await _authService!.AuthenticateAsync(username, password);
            
            if (user != null)
            {
                return true;
            }

            attempts++;
            if (attempts < maxAttempts)
            {
                AnsiConsole.MarkupLine($"[yellow]Intentos restantes: {maxAttempts - attempts}[/]");
            }
        }

        return false;
    }

    static void ShowUserWelcome()
    {
        AnsiConsole.Clear();
        var user = _authService!.GetCurrentUser()!;

        var figlet = new FigletText($"Hola {user.FullName.Split(' ')[0]}!")
            .Centered()
            .Color(Color.Green);

        AnsiConsole.Write(figlet);
        AnsiConsole.WriteLine();

        var userPanel = new Panel(
            $"[cyan]Usuario:[/] {user.Username}\n" +
            $"[cyan]Nombre:[/] {user.FullName}\n" +
            $"[cyan]Rol:[/] {user.Role}\n" +
            $"[cyan]Descripción:[/] [dim]{user.GetRoleDescription()}[/]\n" +
            $"[cyan]Último acceso:[/] {user.LastLogin:yyyy-MM-dd HH:mm:ss}")
        {
            Header = new PanelHeader("[green]?? Información del Usuario[/]"),
            Border = BoxBorder.Rounded,
            Padding = new Padding(2, 1)
        }.BorderStyle(Style.Parse("green"));

        AnsiConsole.Write(userPanel);
        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine("[dim]Presiona cualquier tecla para continuar...[/]");
        Console.ReadKey(true);
    }

    static async Task MainMenu()
    {
        bool exit = false;

        while (!exit)
        {
            AnsiConsole.Clear();
            
            var rule = new Rule("[cyan]?? Menú Principal[/]")
                .RuleStyle("cyan")
                .LeftJustified();
            AnsiConsole.Write(rule);
            AnsiConsole.WriteLine();

            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[cyan]Seleccione una opción:[/]")
                    .PageSize(10)
                    .HighlightStyle(new Style(foreground: Color.Cyan1))
                    .AddChoices(
                        "?? Gestionar Entornos",
                        "?? Desplegar Aplicación",
                        "?? Ver Estado de Aplicaciones",
                        "?? Demostración de Spectre.Console",
                        "?? Ver Perfil de Usuario",
                        "?? Cambiar Entorno",
                        "?? Métricas y Reportes",
                        "?? Cerrar Sesión"));

            AnsiConsole.Clear();

            switch (choice)
            {
                case "?? Gestionar Entornos":
                    await ManageEnvironmentsMenu();
                    break;
                case "?? Desplegar Aplicación":
                    await DeployApplicationMenu();
                    break;
                case "?? Ver Estado de Aplicaciones":
                    await ShowApplicationsStatus();
                    break;
                case "?? Demostración de Spectre.Console":
                    _showcase!.ShowAllDemos();
                    break;
                case "?? Ver Perfil de Usuario":
                    ShowUserProfile();
                    break;
                case "?? Cambiar Entorno":
                    await ChangeEnvironmentMenu();
                    break;
                case "?? Métricas y Reportes":
                    ShowMetricsMenu();
                    break;
                case "?? Cerrar Sesión":
                    exit = ConfirmLogout();
                    break;
            }

            if (!exit)
            {
                AnsiConsole.WriteLine();
                AnsiConsole.MarkupLine("[dim]Presiona cualquier tecla para volver al menú...[/]");
                Console.ReadKey(true);
            }
        }

        AnsiConsole.Clear();
        AnsiConsole.Write(
            new FigletText("Hasta Pronto!")
                .Centered()
                .Color(Color.Yellow));
    }

    static async Task ManageEnvironmentsMenu()
    {
        AnsiConsole.Write(new Rule("[yellow]?? Gestión de Entornos[/]").RuleStyle("grey"));
        AnsiConsole.WriteLine();

        _envService!.DisplayEnvironmentInfo();
        AnsiConsole.WriteLine();

        var environments = Enum.GetValues<DeploymentEnvironment>()
            .Select(e => e.ToString())
            .ToList();

        var selectedEnv = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[cyan]Seleccione un entorno para más información:[/]")
                .AddChoices(environments));

        var env = Enum.Parse<DeploymentEnvironment>(selectedEnv);
        _envService.SetEnvironment(env);
        
        await Task.Delay(500);
        
        AnsiConsole.WriteLine();
        _envService.DisplayEnvironmentInfo();
    }

    static async Task DeployApplicationMenu()
    {
        AnsiConsole.Write(new Rule("[yellow]?? Despliegue de Aplicación[/]").RuleStyle("grey"));
        AnsiConsole.WriteLine();

        var appChoice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[cyan]Seleccione la aplicación a desplegar:[/]")
                .AddChoices(
                    "?? Web API",
                    "?? Mobile App",
                    "??? Desktop App"));

        var envChoice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[cyan]Seleccione el entorno de destino:[/]")
                .AddChoices(Enum.GetValues<DeploymentEnvironment>().Select(e => e.ToString())));

        var appType = appChoice switch
        {
            "?? Web API" => AppType.WebApi,
            "?? Mobile App" => AppType.MobileApp,
            "??? Desktop App" => AppType.DesktopApp,
            _ => AppType.WebApi
        };

        var environment = Enum.Parse<DeploymentEnvironment>(envChoice);

        if (AnsiConsole.Confirm($"[yellow]¿Confirmar despliegue en {environment}?[/]"))
        {
            _appManager!.DeployApplication(appType, environment);
        }
        else
        {
            AnsiConsole.MarkupLine("[red]Despliegue cancelado[/]");
        }

        await Task.CompletedTask;
    }

    static async Task ShowApplicationsStatus()
    {
        AnsiConsole.Write(new Rule("[yellow]?? Estado de Aplicaciones[/]").RuleStyle("grey"));
        AnsiConsole.WriteLine();

        var apps = new[] { AppType.WebApi, AppType.MobileApp, AppType.DesktopApp };

        foreach (var app in apps)
        {
            _appManager!.ShowApplicationStatus(app);
            AnsiConsole.WriteLine();
        }

        await Task.CompletedTask;
    }

    static void ShowUserProfile()
    {
        var user = _authService!.GetCurrentUser()!;

        AnsiConsole.Write(new Rule("[yellow]?? Perfil de Usuario[/]").RuleStyle("grey"));
        AnsiConsole.WriteLine();

        var grid = new Grid();
        grid.AddColumn();
        grid.AddColumn();

        var infoPanel = new Panel(
            $"[bold cyan]Usuario:[/] {user.Username}\n" +
            $"[bold cyan]Nombre Completo:[/] {user.FullName}\n" +
            $"[bold cyan]Rol:[/] {user.Role}\n" +
            $"[bold cyan]Estado:[/] {(user.IsActive ? "[green]Activo[/]" : "[red]Inactivo[/]")}\n" +
            $"[bold cyan]Último Acceso:[/] {user.LastLogin:yyyy-MM-dd HH:mm:ss}")
        {
            Header = new PanelHeader("[cyan]Información Personal[/]"),
            Border = BoxBorder.Rounded
        };

        var permissionsPanel = new Panel(
            user.GetRoleDescription() + "\n\n" +
            "[bold]Permisos:[/]\n" +
            (user.Role == UserRole.Admin ? "[green]?[/]" : "[red]?[/]") + " Acceso total\n" +
            (user.Role != UserRole.Guest ? "[green]?[/]" : "[red]?[/]") + " Desplegar aplicaciones\n" +
            (user.Role == UserRole.Admin || user.Role == UserRole.DevOps ? "[green]?[/]" : "[red]?[/]") + " Acceso a producción")
        {
            Header = new PanelHeader("[yellow]Rol y Permisos[/]"),
            Border = BoxBorder.Rounded
        };

        grid.AddRow(infoPanel, permissionsPanel);
        AnsiConsole.Write(grid);
    }

    static async Task ChangeEnvironmentMenu()
    {
        AnsiConsole.Write(new Rule("[yellow]?? Cambiar Entorno[/]").RuleStyle("grey"));
        AnsiConsole.WriteLine();

        var currentEnv = _envService!.CurrentEnvironment;
        AnsiConsole.MarkupLine($"[cyan]Entorno actual:[/] [{_envService.GetEnvironmentColor()}]{currentEnv}[/]");
        AnsiConsole.WriteLine();

        var envChoice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[cyan]Seleccione el nuevo entorno:[/]")
                .AddChoices(Enum.GetValues<DeploymentEnvironment>().Select(e => e.ToString())));

        var newEnv = Enum.Parse<DeploymentEnvironment>(envChoice);
        _envService.SetEnvironment(newEnv);

        await Task.Delay(500);
        AnsiConsole.WriteLine();
        _envService.DisplayEnvironmentInfo();
    }

    static void ShowMetricsMenu()
    {
        AnsiConsole.Write(new Rule("[yellow]?? Métricas y Reportes[/]").RuleStyle("grey"));
        AnsiConsole.WriteLine();

        _showcase!.ShowBarChart();
        AnsiConsole.WriteLine();

        var table = new Table()
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Cyan1)
            .AddColumn(new TableColumn("[bold]Métrica[/]").Centered())
            .AddColumn(new TableColumn("[bold]Valor[/]").Centered())
            .AddColumn(new TableColumn("[bold]Tendencia[/]").Centered());

        table.AddRow("[cyan]Uptime[/]", "[green]99.9%[/]", "[green]?[/]");
        table.AddRow("[cyan]Errores/hora[/]", "[yellow]12[/]", "[yellow]?[/]");
        table.AddRow("[cyan]Usuarios activos[/]", "[blue]1,543[/]", "[green]?[/]");
        table.AddRow("[cyan]Tiempo respuesta[/]", "[green]234ms[/]", "[green]?[/]");
        table.AddRow("[cyan]Throughput[/]", "[blue]850 req/s[/]", "[green]?[/]");

        AnsiConsole.Write(table);
    }

    static bool ConfirmLogout()
    {
        if (AnsiConsole.Confirm("[yellow]¿Está seguro que desea cerrar sesión?[/]"))
        {
            _authService!.Logout();
            return true;
        }
        return false;
    }
}
