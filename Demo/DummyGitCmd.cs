using System.Collections.Generic;
using Colorful;
using Elchwinkel.CLI;

namespace Demo
{
    internal class DummyGitCmd : CommandWithVerbs
    {
        public override string Name => "git";

        protected override string GetDescriptionHeader() => "A dummy 'git' command to showcase a CommandWithVerbs";

        public override IEnumerable<CommandVerb> GetVerbs()
        {
            yield return new CommandVerb("init", args => Console.WriteLine("..."), "Creates a new Repository");
            yield return new CommandVerb("clone", args => Console.WriteLine("..."), "Clones a Repository from a Remote");
            yield return new CommandVerb("status", args => Console.WriteLine("..."), "Prints the current status of the Repository");
        }
    }
}