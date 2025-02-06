namespace CrapCord_Entities;

public class Packet {
    public readonly PacketOpcode Opcode;
    public readonly DateTime Timestamp;
    public readonly string Payload;
    public readonly string HMAC;
    
    public enum PacketOpcode
    {
        Register,
        Login,
        ViewRooms,
        FetchMessages,
        SendMessage,
        CreateRoom,
        JoinRoom,
        Response,
        KeyHandoff,
        Auth,
        Disconnect = 255
    }

    public Packet(PacketOpcode opcode, DateTime timestamp, string payload, string hmac) {
        Opcode = opcode;
        Timestamp = timestamp;
        Payload = payload;
        HMAC = hmac;
    }
}