using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Elchwinkel.CLI
{
    /// <summary>
    /// Collection of Arguments (<see cref="Arg"/>) that gets passed to a <see cref="ICommand"/>.
    /// Provides convenience Methods to Assert e.g. the correct number of Arguments.
    /// </summary>
    public class Args : IReadOnlyList<Arg>
    {
        private readonly List<Arg> _raw;

        internal Args(IEnumerable<string> raw) => _raw = raw.Select(r=>new Arg(r)).ToList();
        private Args(IEnumerable<Arg> raw) => _raw = raw.ToList();

        internal static Args FromString(string str) => new Args(ArgsHelper.GetArgsPart(str));

        internal Args SkipOne() => new Args(_raw.Skip(1));

        public void AssertExactly(int count)
        {
            if (Count != count) throw new CmdArgException($"Expected exactly {count} arguments but received {this.Count}.");
        }
        public void AssertAtLeast(int count)
        {
            if (Count < count) throw new CmdArgException($"Expected at least {count} arguments but received only {this.Count}.");
        }
        public void AssertAtMost(int count)
        {
            if (Count > count) throw new CmdArgException($"Expected at most {count} arguments but received {this.Count}.");
        }

        public IEnumerator<Arg> GetEnumerator() => _raw.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public int Count => _raw.Count;

        public Arg this[int index]
        {
            get
            {
                AssertAtLeast(index + 1);
                return _raw[index];
            }
        }
    }
}