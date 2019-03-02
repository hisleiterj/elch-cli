using System;
using System.Threading;

namespace Elchwinkel.CLI
{
    /// <summary>
    /// Provides an Implementation for <see cref="ICommand"/> that can be used to define Commands without introducing new Types.
    /// If you prefer having a dedicated class for your Command, derive from <see cref="CommandBase"/> or realize the <see cref="ICommand"/>.
    /// </summary>
    public sealed class Command : CommandBase
    {
        private readonly Action<Args, CancellationToken> _action;
        private readonly string _description;

        public Command(string name, Action<Args, CancellationToken> action, string description = null)
        {
            _action = action;
            _description = description;
            Name = name;
        }
        public Command(string name, Action<Args> action, string description = null)
        {
            _action = (args, token) => action(args);
            Name = name;
            _description = description;
        }
        public override string Name { get; }
        public override string GetDescription(bool verbose) => _description ?? base.GetDescription(verbose);
        public override void Execute(Args args, CancellationToken ct) => _action(args, ct);
    }
}