using System.Net.Sockets;
using CrapCord_Client.GUI;
using CrapCord_Services;
using Microsoft.Extensions.Configuration;
using Veldrid.Sdl2;

namespace CrapCord_Client;

public class ContextManager {
    public Renderer renderer { get; private set; }
    public Sdl2Window window { get; private set; }
    public readonly TcpClient client;
    public readonly PacketSerializer Serializer;
    public readonly IConfigurationRoot config;

    public ContextManager() {
        config = new ConfigurationBuilder().AddUserSecrets<ContextManager>().AddEnvironmentVariables().Build();
        client = new TcpClient();
        Serializer = new PacketSerializer();
        client.Connect("127.0.0.1", int.Parse(config["SOCKET_PORT"] ?? throw new ArgumentNullException("Incorrectly setup config file! Add SOCKET_PORT to your user secrets/environment variables.")));
    }

    public void SetRenderer(Renderer _renderer) {
        renderer = _renderer;
    }

    public void SetWindow(Sdl2Window _window) {
        window = _window;
    }
}