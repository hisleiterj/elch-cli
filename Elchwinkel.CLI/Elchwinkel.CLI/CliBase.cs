using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Elchwinkel.CLI
{
    /// <summary>
    /// Base Type to Implement a Command Line Interface (CLI).
    /// To Implement a custom CLI, derive from this type and provide your <see cref="ICommand"/>s via the <see cref="GetCommands"/> Methods.
    /// </summary>
    public abstract class CliBase : IAutoCompleteHandler
    {
        protected virtual string Prompt => "> ";
        protected virtual string HistoryFile => ".history";
        private CancellationTokenSource _cts;
        private ColorScheme _colorScheme;
        private bool _shutdownRequested;

        protected CliBase()
        {
            
        }

        /// <summary>
        /// Gets or Sets the <see cref="ColorScheme"/> the CLI should use.
        /// The <see cref="ColorScheme"/> defines which <see cref="Color"/>s are used for the Terminal Background and various Text Colors.
        /// </summary>
        public ColorScheme ColorScheme
        {
            get => _colorScheme;
            set
            {
                _colorScheme = value;
                Colorful.Console.BackgroundColor = value.Background;
                Console.Clear();
                Colorful.Console.ForegroundColor = value.Color1;
            }
        }


        private void _Initialize()
        {
            Console.OutputEncoding = Encoding.UTF8;
            _TryLoadHistory();
            ReadLine.AutoCompletionHandler = this;
            Console.CancelKeyPress += (sender, args) =>
            {
                args.Cancel = _cts != null;
                _cts?.Cancel();
            };
            OnAfterInitialization();
        }

        protected virtual void OnAfterInitialization() { }

        /// <summary>
        /// Queries the available <see cref="ICommand"/>s.
        /// This Methods gets called often - It is recommended to not create/build up commands every time the Method is called but instead cache the Commands and re-use them.
        /// </summary>
        /// <returns>
        /// All the <see cref="ICommand"/>s the User should be able to Execute.
        /// The <see cref="ICommand.Name"/>s of the Commands must be unique (case insensitive)."/>
        /// </returns>
        protected abstract IEnumerable<ICommand> GetCommands();

        protected ICommand HelpCommand => new Command("help", args =>
        {
            foreach (var command in GetCommands())
            {
                Colorful.Console.WriteLine(command.Name, ColorScheme.Color1);
                Colorful.Console.WriteLine("     " + command.GetDescription(false), ColorScheme.Color2);
            }
        }, "Displays information about all available Commands.");
        protected ICommand ExitCommand => new Command("quit", args => RequestShutdown(), "Quits this Program");

        /// <summary>
        /// Request the Shutdown of the CLI (not necessarily your main Program).
        /// Note: In some cases calling this method does not immediately shutdown the CLI but a Newline Input from the User may be required.
        /// This is usually not a problem because the use case is shutting down the CLI by user-request, e.g. using the <see cref="ExitCommand"/>.
        /// </summary>
        public void RequestShutdown()
        {
            MaintainHistory();
            OnShutdown();
            _shutdownRequested = true;
        }
        /// <summary>
        /// Must be Called to Run the Command Line Interface, i.e. allows the User to input Names and Arguments in order to execute <see cref="ICommand"/>s.
        /// After Calling this (blocking) Method, no other Thread should interfere with the Console Input/Output.
        /// </summary>
        public void Run()
        {
            ColorScheme = ColorScheme.Dark;
            _Initialize();
            while (!_shutdownRequested)
            {
                var input = ReadLine.Read(Prompt);
                if (string.IsNullOrEmpty(input)) continue;
                ReadLine.AddHistory(input);
                _ExecuteCmd(input);
            }
        }

        /// <summary>
        /// Called to run the Command Line Interface in non-interactive mode, i.e. executes a single <see cref="ICommand"/> and then exits.
        /// </summary>
        public void RunNonInteractive(string input)
        {
            _ExecuteCmd(input);
        }

        protected virtual void OnCommandCanceled(ICommand cmd) 
            => Colorful.Console.WriteLine("Command was Cancelled by User Request.", ColorScheme.Color3);

        protected virtual void OnUnhandledCommandException(ICommand cmd, Exception e)
        {
            Colorful.Console.WriteLine("Error Executing Cmd: ", ColorScheme.Error);
            Console.WriteLine(e.ToString());
        }
        protected virtual void OnCmdArgumentException(ICommand cmd, Exception e)
        {
            Colorful.Console.WriteLine(e.Message, ColorScheme.Error);
        }
        protected virtual void UnknownCommandHandler(string userInput)
            => Console.WriteLine($"Unknown Command '{userInput}'", ColorScheme.Error);


        private void _ExecuteCmd(string input)
        {
            var cmdPart = ArgsHelper.GetCommandPart(input);
            var cmd = GetCommands().FirstOrDefault(command => command.Name.Equals(cmdPart, StringComparison.OrdinalIgnoreCase));
            if (cmd == null)
            {
                UnknownCommandHandler(cmdPart);
                return;
            }

            try
            {
                _cts = new CancellationTokenSource();
                cmd.Execute(Args.FromString(input), _cts.Token);
            }
            catch (CmdArgException e)
            {
                OnCmdArgumentException(cmd, e);
            }
            catch (OperationCanceledException)
            {
                OnCommandCanceled(cmd);
            }
            catch (AggregateException e)
            {
                if (e.InnerExceptions.Any(inner => inner is OperationCanceledException))
                    OnCommandCanceled(cmd);
                else if (e.InnerExceptions.Any(inner => inner is CmdArgException))
                    OnCmdArgumentException(cmd, e.InnerExceptions.First(x => x is CmdArgException));
                else
                    OnUnhandledCommandException(cmd, e);
            }
            catch (Exception e)
            {
                OnUnhandledCommandException(cmd, e);
            }
            finally
            {
                _cts = null;
            }
        }

        protected void MaintainHistory() => _SaveHistory();

        private void _SaveHistory()
        {
            var history = ReadLine.GetHistory();
            history = history.Skip(Math.Max(history.Count - MaxHistoryEntries, 0)).ToList(); 
            File.WriteAllLines(HistoryFile, history);
        }

        protected virtual int MaxHistoryEntries => 100;

        private void _TryLoadHistory()
        {
            if (!File.Exists(HistoryFile)) return;
            try
            {
                ReadLine.AddHistory(File.ReadAllLines(HistoryFile));
            }
            catch (Exception)
            {
                //ignore by design
            }
        }

        char[] IAutoCompleteHandler.Separators { get; set; } = { ' ', '.', '/', '\\', ':' };

        string[] IAutoCompleteHandler.GetSuggestions(string text, int index)
        {
            Debug.WriteLine($"text: '{text}' index: {index}");
            var cmdName = ArgsHelper.GetCommandPart(text);
            var suggestions = _AutocompleteSuggestions(text);
            if (suggestions.Count >= 1 && text.Contains(" "))
            {
                if (!(GetCommands().FirstOrDefault(c => c.Name.Equals(cmdName, StringComparison.OrdinalIgnoreCase)) is ISupportsAutocomplete cmd))
                    return Array.Empty<string>();
                return cmd.GetSuggestions(ArgsHelper.GetArgsPart(text).FirstOrDefault() ?? String.Empty, index);
            }
            return suggestions.ToArray();
        }
        private IReadOnlyList<string> _AutocompleteSuggestions(string text)
        {
            return GetCommands().Select(cmd => cmd.Name).Where(name => text.StartsWith(name, StringComparison.OrdinalIgnoreCase) || name.StartsWith(text, StringComparison.OrdinalIgnoreCase)).ToArray();
        }

        protected virtual void OnShutdown() { }
    }
}