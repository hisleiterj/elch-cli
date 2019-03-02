using System;
using System.Threading;
using System.Threading.Tasks;

namespace Elchwinkel.CLI
{
    /// <summary>
    /// Async Variant of <see cref="Command"/>. Refer to <see cref="Command"/> for more information.
    /// </summary>
    public sealed class AsyncCommand : AsyncCommandBase
    {
        private readonly Func<Args, CancellationToken, Task> _action;
        private readonly string _description;

        public AsyncCommand(string name, Func<Args, CancellationToken, Task> action, string description = null)
        {
            _action = action;
            _description = description;
            Name = name;
        }

        public override string Name { get; }
        public override string GetDescription(bool verbose) => _description ?? base.GetDescription(verbose);
        public override Task ExecuteAsync(Args args, CancellationToken ct) => _action(args, ct);
    }
}