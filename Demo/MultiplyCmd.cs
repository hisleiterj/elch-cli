using System;
using System.Threading;
using Elchwinkel.CLI;

namespace Demo
{
    internal class MultiplyCmd : CommandBase
    {
        public override string Name => "mult";
        public override void Execute(Args args, CancellationToken ct)
        {
            args.AssertExactly(2);
            var x = args[0].AsDouble();
            var y = args[1].AsDouble();
            Console.WriteLine($"{x} * {y} = {x * y}");
        }
    }
}