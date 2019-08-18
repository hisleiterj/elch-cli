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
            using (var pb = new ProgressBar())
            {
                for (var i = 0; i < 100; i++)
                {
                    await Task.Delay(100, ct);
                    ct.ThrowIfCancellationRequested();
                    pb.Report(i/100.0);
                }
                Console.WriteLine("Done");
            }
           
        }

        public override string GetDescription(bool verbose) => "Show-cases the Cancellation Feature.";
    }
}