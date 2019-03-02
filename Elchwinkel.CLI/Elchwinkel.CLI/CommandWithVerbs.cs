using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;

namespace Elchwinkel.CLI
{
    /// <summary>
    /// Realizes the <see cref="ICommand"/> interface for Commands that group a "collection" of similar Commands under one name.
    /// A typical example in the real world is the "git" Command that can be invoked with different "verbs", e.g. "git init", "git clone" etc.
    /// This Base Type makes it easy to implement such kind of Commands that provide different "sub-commands".
    /// For simpler Commands, consider realizing the <see cref="ICommand"/> Interface directly or deriving from <see cref="CommandBase"/>.
    /// </summary>
    public abstract class CommandWithVerbs :
        CommandBase,
        ISupportsAutocomplete
    {
        /// <inheritdoc />
        public override string GetDescription(bool verbose)
        {
            return !verbose
                ? GetDescriptionHeader()
                : GetDescriptionHeader() +
                  Environment.NewLine +
                  String.Join(Environment.NewLine,
                      GetVerbs().Select(verb => $"   - {Name} {verb.Name}: {verb.Description ?? "n/a"}"));
        }

        protected virtual string GetDescriptionHeader() => String.Empty;
        public abstract IEnumerable<CommandVerb> GetVerbs();
        public virtual Action<Args> DefineDefaultAction() => args => Console.Write(GetDescription(true));
        public abstract override string Name { get; }

        public override void Execute(Args args, CancellationToken ct)
        {
            if (args.Count == 0)
            {
                var defaultAction = DefineDefaultAction();
                if(defaultAction == null) throw new ArgumentException("Invalid Argument.");
                defaultAction(args.SkipOne());
                return;
            }

            if (args[0].Equals("?") || args[0].Equals("help"))
            {
                 Colorful.Console.WriteLine("     " + this.GetDescription(true), Color.CadetBlue);
                return;
            }
            var verb = _MatchVerb(args[0]);
            if (verb == null) throw new ArgumentException("Invalid Argument.");
            verb.Action(args.SkipOne());
        }


        private CommandVerb _MatchVerb(string str)
        {
            return GetVerbs().FirstOrDefault(verb => verb.Name.Equals(str, StringComparison.OrdinalIgnoreCase));
        }

        string[] ISupportsAutocomplete.GetSuggestions(string text, int index)
        {
            return GetVerbs().Select(v => v.Name).Where(s => s.StartsWith(text, StringComparison.OrdinalIgnoreCase)).ToArray();
        }
    }
}