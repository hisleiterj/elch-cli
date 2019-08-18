using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
                new ListUsers(),
                new Command("menu-demo", args =>
                {
                    var menu = new Menu(new []
                    {
                        new Menu.Item("Hi", 1), 
                        new Menu.Item("Foo", 2), 
                        new Menu.Item("Bar", 3), 
                        new Menu.Item("Buz", 4), 
                    });
                    //var selected = menu.Show();
                    var foo = Menu.Show("option a", "option b");
                    Console.WriteLine($"Selected: {foo}");
                    //Console.WriteLine($"Selected: {selected}");
                }), 
                new Command("echo", args => { Console.WriteLine($"You said: '{args[0]}'"); }),
                ExitCommand
            };
        }



        protected string[] Autocomplete(IEnumerable<string> strings, string text)
        {
            return strings.Where(s => s.StartsWith(text, StringComparison.OrdinalIgnoreCase)).ToArray();
        }
        protected string[] AutocompleteFileSystem(string text)
        {
            var isRooted = Path.IsPathRooted(text);

            var x = Directory.GetFileSystemEntries(text);
            return x.Select(s => s.Split(Path.DirectorySeparatorChar).Last()).ToArray();
            //return x;
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