using System.Threading;

namespace Elchwinkel.CLI
{
    /// <summary>
    /// Any custom Command must realize this interface,
    /// either directly or by inheriting from <see cref="CommandBase"/>, <see cref="AsyncCommandBase"/> or <see cref="CommandWithVerbs"/>.
    /// If you prefer to not introduce new Types for your Commands, consider using <see cref="Command"/> or <see cref="AsyncCommand"/>.
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// The Name the User needs to type in to Execute this Command.
        /// Must not contain any Whitespace characters.
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Executes the Command with the specified Arguments (<see cref="Args"/>).
        /// </summary>
        /// <param name="args">The collection of Arguments (<see cref="Args"/>) the User provided to this command.</param>
        /// <param name="ct">The <see cref="CancellationToken"/> for (optional) Cancellation Handling of the Command.</param>
        void Execute(Args args, CancellationToken ct);
        /// <summary>
        /// A (human readable) Description of the Command used to display help information for the Command.
        /// </summary>
        /// <param name="verbose"></param>
        /// <returns></returns>
        string GetDescription(bool verbose);
    }
}