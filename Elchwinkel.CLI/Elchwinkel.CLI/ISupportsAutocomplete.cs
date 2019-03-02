namespace Elchwinkel.CLI
{
    /// <summary>
    /// A <see cref="ICommand"/> may realize this interfaces to handle Tab Auto-Completion of Command Arguments.
    /// Auto-Completion for the Command Name itself works out of the box.
    /// </summary>
    public interface ISupportsAutocomplete
    {
        /// <summary>
        /// Queries the Suggestions the Command offers given the <see cref="text"/> input from the user.
        /// </summary>
        /// <param name="text">The text the User has typed</param>
        /// <param name="index">The Current Cursor Position</param>
        /// <returns>An Array of Suggestions that the User can iterate via the Tab-Key. Array may be empty.</returns>
        string[] GetSuggestions(string text, int index);
    }
}