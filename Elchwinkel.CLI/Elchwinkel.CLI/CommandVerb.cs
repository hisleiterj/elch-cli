using System;
using System.Threading.Tasks;

namespace Elchwinkel.CLI
{
    /// <summary>
    /// A kind of Sub-Command used only in the context of a <see cref="CommandWithVerbs"/>.
    /// </summary>
    public class CommandVerb
    {
        public CommandVerb(string name, Action<Args> action, string description = null)
        {
            Name = name;
            Action = action;
            Description = description;
        }

        public CommandVerb(string name, Func<Args, Task> action, string description = null)
        {
            Name = name;
            Action = args=>action(args).Wait();
            Description = description;
        }

        public string Name { get; }
        public Action<Args> Action { get; }
        public string Description { get; }
    }
}