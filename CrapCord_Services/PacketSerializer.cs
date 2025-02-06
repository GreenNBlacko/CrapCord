using System.Text;
using Google.Protobuf.CrapCord.PacketToProto;
using CrapCord_Entities;
using Google.Protobuf;

namespace CrapCord_Services;

/// <summary>
/// Class to (de)serialize packets before sending/after receiving them. Made using Google Protocol Buffers
/// </summary>
public class PacketSerializer {
    /// <summary>
    ///     Function to serialize packet contents
    /// </summary>
    /// <param name="packet">Packet to serialize</param>
    /// <returns>A <see cref="byte"/> array</returns>
    /// <exception cref="ArgumentException">Thrown if the packet creation is unsuccessful</exception>
    /// <exception cref="NullReferenceException">Thrown if the serialization fails</exception>
    /// <seealso cref="Deserialize"/>>
    public byte[] Serialize(Packet packet) {
        using MemoryStream ms = new();
        var prPacket = new PacketProto() {
            Opcode = (PacketProto.Types.PacketOpcode)(int)packet.Opcode,
            Timestamp = packet.Timestamp.Ticks.ToString(),
            Payload = packet.Payload,
            HMAC = packet.HMAC,
        } ?? throw new ArgumentException("Packet format is invalid");

        prPacket.WriteTo(ms);

        return ms.ToArray() ?? throw new NullReferenceException("Serialization failure");
    }

    /// <summary>
    ///     Function to deserialize received packet
    /// </summary>
    /// <param name="data">The data received</param>
    /// <returns><see cref="Packet"/></returns>
    /// <exception cref="ArgumentException">Thrown if the received packet is corrupted</exception>
    /// <seealso cref="Serialize"/>
    public Packet Deserialize(byte[] data) {
        using MemoryStream ms = new(data);
        var prPacket = PacketProto.Parser.ParseFrom(ms) ?? throw new ArgumentException("Packet format is invalid");
        
        return new Packet(
            (Packet.PacketOpcode)(int)prPacket.Opcode, 
            new DateTime(long.Parse(prPacket.Timestamp)), 
            prPacket.Payload, 
            prPacket.HMAC);
    }
}