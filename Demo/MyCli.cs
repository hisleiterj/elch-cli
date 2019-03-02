using System;
using System.Collections.Generic;
using Elchwinkel.CLI;

namespace Demo
{
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
                new Command("echo", args => { Console.WriteLine($"You said: '{args[0]}'"); }),
                ExitCommand
            };
        }
        protected override void OnAfterInitialization()
        {
            Colorful.Console.WriteAscii("DEMO CLI", ColorScheme.Color2);
            Console.WriteLine("This is Demo CLI V1.0");
            Console.WriteLine("Enter 'help' for information about available commands.");
            Console.WriteLine();
        }
        protected override IEnumerable<ICommand> GetCommands() => _commands;
    }
}