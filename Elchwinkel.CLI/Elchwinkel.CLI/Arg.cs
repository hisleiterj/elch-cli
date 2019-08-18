using System;
using System.Globalization;

namespace Elchwinkel.CLI
{
    /// <summary>
    /// Represents an Argument the User provided to a <see cref="ICommand"/>.
    /// The underlying type is always <see cref="string"/>, but easy Access to <see cref="int"/> and <see cref="double"/> representations are provided.
    /// </summary>
    public class Arg : IEquatable<string>
    {
        public string Raw { get; }
        public Arg(string raw) => Raw = raw;

        public static implicit operator string(Arg x) => x.Raw;
        public static Arg None => new Arg(null);

        public bool Equals(string other) => String.Equals(Raw, other, StringComparison.OrdinalIgnoreCase);

        public override string ToString() => Raw;

        public double AsDouble()
        {
            try
            {
                return double.Parse(Raw, NumberStyles.Number);
            }
            catch (Exception)
            {
                throw new CmdArgException($"Invalid Argument. '{Raw}' does not appear to be a floating point number.");
            }
        }
        public int AsInt()
        {
            try
            {
                if (Raw.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
                    return Convert.ToInt32(Raw, 16);
                if (Raw.StartsWith("0b", StringComparison.OrdinalIgnoreCase))
                    return Convert.ToInt32(Raw.Substring(2), 2);
                return int.Parse(Raw, NumberStyles.Number);
            }
            catch (Exception)
            {
                throw new CmdArgException($"Invalid Argument. '{Raw}' does not appear to be a integer number.");
            }
        }
    }
}