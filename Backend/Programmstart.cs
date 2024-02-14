using System;
using System.Threading.Tasks;

namespace TorGuard
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Controller controller = new Controller();
            await controller.StartSystemAsync();
            // Weitere Initialisierungen können hier erfolgen.
        }
    }
}