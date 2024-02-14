using TorGuard.Backend;
using TorGuard;
static async Task Main(string[] args)
{
    Controller controller = new Controller();
    await controller.StartSystemAsync();
    // Weitere Initialisierungen können hier erfolgen.
}
