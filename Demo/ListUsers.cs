using System;
using System.Threading;
using Elchwinkel.CLI;

namespace Demo
{
    internal class ListUsers : CommandBase
    {
        public override string Name => "users";
        public override string GetDescription(bool verbose) => "Demo for a simple table";

        public override void Execute(Args args, CancellationToken ct)
        {
            var table = new Table("Id", "First Name", "Last Name");
            table.AddRow("1", "John", "Doe");
            table.AddRow("2", "Jane", "Smith");
            table.Write();
        }
    }
}