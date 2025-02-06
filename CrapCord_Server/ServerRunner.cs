using CrapCord_Server.Services;

namespace CrapCord_Server;

class ServerRunner {
    static async Task Main(string[] args) {
        var instance = new ThreadManager();

        await instance.MainLoop();
    }
}