using CrapCord_Server.Database;
using CrapCord_Server.IO;
using CrapCord_Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using ArgumentNullException = System.ArgumentNullException;

namespace CrapCord_Server;

/// <summary>
///     A utility class meant for storing values relevant to the server's operation
/// </summary>
/// References: <br/>
/// <see cref="IConfigurationRoot"/> <br/>
/// <see cref="DBMS"/> <br/>
/// <see cref="PacketSerializer"/>
public class Context {
    public readonly IConfigurationRoot Config;
    public readonly DBMS Database;
    public readonly PacketSerializer Serializer;
    public readonly Logger Logger;
    
    /// <summary>
    /// Value initialization
    /// </summary>
    /// <exception cref="ArgumentNullException">Thrown if the config is not set up properly(e.g.: missing values)</exception>
    public Context() {
        Logger = new();
        Config = new ConfigurationBuilder().AddUserSecrets<ServerRunner>().AddEnvironmentVariables().Build();
        Logger.Log($"Starting CrapCord server v{Config["SERVER_VERSION"]}");
        Logger.Log("Logger initialized");
        Serializer = new PacketSerializer();
        Logger.Log("Packet serializer initialized");
        Database = new DBMS(
            Config["DB_IP"] ?? throw new ArgumentNullException($"Bad config! Missing database IP, add DB_IP to your environment variables or user secrets."),
            Config["DB_PORT"] ?? throw new ArgumentNullException($"Bad config! Missing database port, add DB_PORT to your environment variables or user secrets."),
            Config["DB_NAME"] ?? throw new ArgumentNullException($"Bad config! Missing database name, add DB_NAME to your environment variables or user secrets."),
            Config["DB_USERNAME"] ?? throw new ArgumentNullException($"Bad config! Missing database username, add DB_USERNAME to your environment variables or user secrets."),
            Config["DB_PASSWORD"] ?? throw new ArgumentNullException($"Bad config! Missing database password, add DB_PASSWORD to your environment variables or user secrets.")
        );
        Logger.Log("Database initialized successfully");
        Logger.Log("Context initialized successfully! Waiting for server...");
    }
}