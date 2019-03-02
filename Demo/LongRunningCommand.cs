using System;
using System.Threading;
using System.Threading.Tasks;
using Elchwinkel.CLI;

namespace Demo
{
    internal class LongRunningCommand : AsyncCommandBase
    {
        public override string Name => "long-running";
        public override async Task ExecuteAsync(Args args, CancellationToken ct)
        {
            for (var i = 0; i < 10; i++)
            {
                await Task.Delay(1000, ct);
                ct.ThrowIfCancellationRequested();
                Console.WriteLine($"Progress: {i}/10");
            }
            Console.WriteLine("Done");
        }

        public override string GetDescription(bool verbose) => "Show-cases the Cancellation Feature.";
    }
}