using System.Net.Sockets;
using System.Text;
using CrapCord_Client.GUI;
using CrapCord_Entities;
using static CrapCord_Entities.Packet;

namespace CrapCord_Client;

/// <summary>
///     Entry class that handles startup and shutdown of the system
/// </summary>
public class ClientRunner {
    private Renderer gui;
    private ContextManager ctx;

    public static void Main() {
        var instance = new ClientRunner();

        // Handle shutdown
        Console.CancelKeyPress += instance.OnProcessExit;
        AppDomain.CurrentDomain.ProcessExit += instance.OnProcessExit;

        // Entry point
        instance.Start().Wait();
    }

    private async Task Start() {
        ctx = new ContextManager();

        gui = new Renderer(ctx);

        while (true)
            await Task.Yield();
    }

    private void OnProcessExit(object? sender, EventArgs e) {
        if (!ctx.client.Connected) return;
        try {
            NetworkStream stream = ctx.client.GetStream();
            var packet = new Packet(PacketOpcode.Disconnect, DateTime.Now, "Goodbye, world!", "");

            var data = ctx.Serializer.Serialize(packet);
            stream.Write(data, 0, data.Length);

            ctx.client.Close();
        }
        catch (Exception ex) {
            Console.WriteLine($"Error during disconnect: {ex.Message}");
        }
        finally {
            Environment.Exit(0); // Ensure process terminates cleanly
        }
    }
}