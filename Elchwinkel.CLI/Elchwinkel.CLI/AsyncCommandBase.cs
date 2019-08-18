using System.Threading;
using System.Threading.Tasks;

namespace Elchwinkel.CLI
{
    /// <summary>
    /// Async Variant of <see cref="CommandBase"/>. Refer to <see cref="CommandBase"/> for more information.
    /// </summary>
    public abstract class AsyncCommandBase : ICommand
    {
        public abstract string Name { get; }
        public void Execute(Args args, CancellationToken ct) => ExecuteAsync(args, ct).GetAwaiter().GetResult();
        public abstract Task ExecuteAsync(Args args, CancellationToken ct);
        public virtual string GetDescription(bool verbose) => "n/a";
    }
}