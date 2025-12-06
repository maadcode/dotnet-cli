using Spectre.Console;

namespace ConsoleAppInteractiveNugetTool.Services;

public class SpectreShowcase
{
    public void ShowBarChart()
    {
        AnsiConsole.Write(new Rule("[yellow]?? Gráfico de Barras[/]").RuleStyle("grey"));
        AnsiConsole.WriteLine();

        var chart = new BarChart()
            .Width(60)
            .Label("[bold underline]Rendimiento por Aplicación[/]")
            .CenterLabel()
            .AddItem("Web API", 85, Color.Green)
            .AddItem("Mobile App", 72, Color.Blue)
            .AddItem("Desktop App", 93, Color.Yellow)
            .AddItem("Microservices", 68, Color.Orange1)
            .AddItem("Database", 95, Color.Red);

        AnsiConsole.Write(chart);
        AnsiConsole.WriteLine();
    }

    public void ShowProgress()
    {
        AnsiConsole.Write(new Rule("[yellow]? Barra de Progreso[/]").RuleStyle("grey"));
        AnsiConsole.WriteLine();

        AnsiConsole.Progress()
            .AutoClear(false)
            .Columns(
                new TaskDescriptionColumn(),
                new ProgressBarColumn(),
                new PercentageColumn(),
                new RemainingTimeColumn(),
                new SpinnerColumn())
            .Start(ctx =>
            {
                var task1 = ctx.AddTask("[green]Procesando datos[/]");
                var task2 = ctx.AddTask("[blue]Generando reportes[/]");
                var task3 = ctx.AddTask("[yellow]Enviando notificaciones[/]");

                while (!ctx.IsFinished)
                {
                    task1.Increment(1.5);
                    task2.Increment(1.0);
                    task3.Increment(0.5);
                    Thread.Sleep(50);
                }
            });

        AnsiConsole.WriteLine();
    }

    public void ShowCalendar()
    {
        AnsiConsole.Write(new Rule("[yellow]?? Calendario[/]").RuleStyle("grey"));
        AnsiConsole.WriteLine();

        var calendar = new Calendar(DateTime.Now.Year, DateTime.Now.Month)
            .AddCalendarEvent(DateTime.Now)
            .AddCalendarEvent(DateTime.Now.AddDays(3))
            .AddCalendarEvent(DateTime.Now.AddDays(7))
            .HeaderStyle(Style.Parse("blue bold"))
            .HighlightStyle(Style.Parse("yellow bold"));

        AnsiConsole.Write(calendar);
        AnsiConsole.WriteLine();
    }

    public void ShowFiglet()
    {
        AnsiConsole.Write(
            new FigletText("Spectre Console")
                .LeftJustified()
                .Color(Color.Cyan1));
    }

    public void ShowMarkup()
    {
        AnsiConsole.Write(new Rule("[yellow]?? Markup y Estilos[/]").RuleStyle("grey"));
        AnsiConsole.WriteLine();

        AnsiConsole.MarkupLine("[bold yellow]Texto en negrita y amarillo[/]");
        AnsiConsole.MarkupLine("[italic green]Texto en cursiva y verde[/]");
        AnsiConsole.MarkupLine("[underline red]Texto subrayado y rojo[/]");
        AnsiConsole.MarkupLine("[bold italic blue on yellow]Combinación de estilos[/]");
        AnsiConsole.MarkupLine("[link=https://spectreconsole.net]Spectre.Console Website[/]");
        AnsiConsole.WriteLine();

        var gradient = new TextPath("C:/Users/Documents/important-file.txt")
            .RootStyle(new Style(foreground: Color.Red))
            .SeparatorStyle(new Style(foreground: Color.Green))
            .StemStyle(new Style(foreground: Color.Blue))
            .LeafStyle(new Style(foreground: Color.Yellow));

        AnsiConsole.Write(gradient);
        AnsiConsole.WriteLine();
        AnsiConsole.WriteLine();
    }

    public void ShowGrid()
    {
        AnsiConsole.Write(new Rule("[yellow]?? Grid Layout[/]").RuleStyle("grey"));
        AnsiConsole.WriteLine();

        var grid = new Grid();
        grid.AddColumn();
        grid.AddColumn();
        grid.AddColumn();

        grid.AddRow(
            new Panel("[green]Panel 1[/]").BorderColor(Color.Green),
            new Panel("[blue]Panel 2[/]").BorderColor(Color.Blue),
            new Panel("[red]Panel 3[/]").BorderColor(Color.Red));

        grid.AddRow(
            new Panel("[yellow]Panel 4[/]").BorderColor(Color.Yellow),
            new Panel("[purple]Panel 5[/]").BorderColor(Color.Purple),
            new Panel("[cyan]Panel 6[/]").BorderColor(Color.Cyan1));

        AnsiConsole.Write(grid);
        AnsiConsole.WriteLine();
    }

    public void ShowLiveDisplay()
    {
        AnsiConsole.Write(new Rule("[yellow]?? Live Display[/]").RuleStyle("grey"));
        AnsiConsole.WriteLine();

        var table = new Table().Centered();

        AnsiConsole.Live(table)
            .AutoClear(false)
            .Overflow(VerticalOverflow.Ellipsis)
            .Cropping(VerticalOverflowCropping.Top)
            .Start(ctx =>
            {
                table.AddColumn("Servidor");
                ctx.Refresh();
                Thread.Sleep(500);

                table.AddColumn("Estado");
                ctx.Refresh();
                Thread.Sleep(500);

                table.AddColumn("CPU");
                ctx.Refresh();
                Thread.Sleep(500);

                for (int i = 1; i <= 5; i++)
                {
                    table.AddRow(
                        $"[cyan]Server-{i}[/]",
                        $"[green]Online[/]",
                        $"[yellow]{Random.Shared.Next(10, 90)}%[/]");
                    ctx.Refresh();
                    Thread.Sleep(300);
                }
            });

        AnsiConsole.WriteLine();
    }

    public void ShowAllDemos()
    {
        ShowFiglet();
        AnsiConsole.WriteLine();
        
        if (AnsiConsole.Confirm("¿Ver todas las demostraciones?", false))
        {
            ShowMarkup();
            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine("[dim]Presiona cualquier tecla para continuar...[/]");
            Console.ReadKey(true);
            AnsiConsole.Clear();

            ShowBarChart();
            AnsiConsole.MarkupLine("[dim]Presiona cualquier tecla para continuar...[/]");
            Console.ReadKey(true);
            AnsiConsole.Clear();

            ShowGrid();
            AnsiConsole.MarkupLine("[dim]Presiona cualquier tecla para continuar...[/]");
            Console.ReadKey(true);
            AnsiConsole.Clear();

            ShowCalendar();
            AnsiConsole.MarkupLine("[dim]Presiona cualquier tecla para continuar...[/]");
            Console.ReadKey(true);
            AnsiConsole.Clear();

            ShowProgress();
            AnsiConsole.MarkupLine("[dim]Presiona cualquier tecla para continuar...[/]");
            Console.ReadKey(true);
            AnsiConsole.Clear();

            ShowLiveDisplay();
        }
    }
}
