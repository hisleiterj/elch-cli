using System.Threading;
using System.Threading.Tasks;
using Colorful;

namespace Demo
{
    class EntryPoint
    {
        static void Main(string[] args)
        {
            Console.Title = "My CLI";
            new MyCli().Run();
        }
    }
}
