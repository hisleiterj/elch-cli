# elch-cli
A .NET/C# Library to Build your own custom Command Line Interfaces (CLI) with convenient features like Tab-Complete and Command History.
## Usage
```c#
static void Main(string[] args)
{
    Console.Title = "My CLI";
    new MyCli().Run();
}
internal class MyCli : CliBase
{
    private readonly ICommand[] _commands;
    public MyCli()
    {
        _commands = new[]
        {
            HelpCommand,
            new MultiplyCmd(),
            new DummyGitCmd(),
            new LongRunningCommand(),
            new Command("echo", args => { System.Console.WriteLine($"You said: '{args[0]}'"); }),
            ExitCommand
        };
    }
    protected override void OnAfterInitialization()
    {
        Colorful.Console.WriteAscii("DEMO CLI", ColorScheme.Color2);
        System.Console.WriteLine("This is Demo CLI V1.0");
        System.Console.WriteLine("Enter 'help' for information about available commands.");
        System.Console.WriteLine();
    }
    protected override IEnumerable<ICommand> GetCommands() => _commands;
}
```
