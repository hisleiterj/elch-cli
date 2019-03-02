using System;
using System.Threading;

namespace Elchwinkel.CLI
{
    /// <summary>
    /// Abstract Command that realizes the <see cref="ICommand"/> interface.
    /// Derive from this Type to implement you own Commands. Refer also to documentation of <see cref="ICommand"/>.
    /// </summary>
    public abstract class CommandBase : ICommand
    {
        public abstract string Name { get; }
        public abstract void Execute(Args args, CancellationToken ct);
        public virtual string GetDescription(bool verbose) => "n/a";
    }
}