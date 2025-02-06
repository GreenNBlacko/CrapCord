using System.Net.Sockets;
using System.Text;
using CrapCord_Entities;
using static CrapCord_Server.IO.Logger;
using static CrapCord_Entities.Packet;

namespace CrapCord_Server.Protocol;

/// <summary>
///     Communication with the client thread
/// </summary>
public class ServerTCP {
    protected readonly TcpClient _client;
    protected readonly Context _ctx;
    protected NetworkStream _ns;

    public ServerTCP(TcpClient client, Context ctx) {
        _client = client;
        _ctx = ctx;
    }

    /// <summary>
    ///     Listens for packets from the client
    /// </summary>
    /// <param name="token">A cancellation token to (gracefully) cut communications with the client in the event of a server shutdown</param>
    public async Task Listen(CancellationToken token) {
        _ns = _client.GetStream();
        
        _ctx.Logger.Log($"Awaiting packets from {_client.Client.RemoteEndPoint}");
        
        while (_client.Connected) {
            while (!_ns.DataAvailable) { // Wait until client establishes communcations
                await Task.Yield();
                
                if (token.IsCancellationRequested)
                    goto cancel;
            }

            if (token.IsCancellationRequested)
                goto cancel;
            
            _ctx.Logger.Log($"Received a packet from {_client.Client.RemoteEndPoint}");
            
            var packet = ReadPacket();

            var response = await ParsePacket(packet);
            
            if(response == 255)
                goto cancel;

            if (response != 200) {
                _ctx.Logger.Log("Encountered a problem with parsing the packet", LogSeverity.Error);
            }
            
            continue;
            
            cancel:
            _ctx.Logger.Log($"Client disconnected: {_client.Client.RemoteEndPoint}");
            _client.Close();
            break;
        }
    }

    /// <summary>
    ///     Reads the packet received from the client
    /// </summary>
    protected Packet ReadPacket() {
        var bytes = new List<byte>();

        int read = 0;

        while ((read = _ns.ReadByte()) != -1) {
            bytes.Add(Convert.ToByte(read));
        }

        var buffer = bytes.ToArray();
        var packet = _ctx.Serializer.Deserialize(buffer) ?? throw new NullReferenceException("Packet is corrupted");
        
        return packet;
    }

    /// <summary>
    ///     Parses the received packet and appropriately processes any request denoted in it
    /// </summary>
    /// <param name="packet"></param>
    /// <returns></returns>
    protected async Task<int> ParsePacket(Packet packet) {
        switch (packet.Opcode) {
            case PacketOpcode.Disconnect:
                _ctx.Logger.Log("Client is disconnecting");
                return 255;
            
            default:
                // Fall through
                _ctx.Logger.Log("Unsupported opcode! Try again later.", LogSeverity.Warning);
                break;
        }
        return 200;
    }
}