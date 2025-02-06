using System.Collections.Concurrent;
using System.Net.Sockets;
using CrapCord_Server.Protocol;

namespace CrapCord_Server.Services;

public class ThreadManager {
    private ConcurrentDictionary<CancellationTokenSource, Thread> _threads;
    private TcpListener _server;
    private Context _ctx;

    public ThreadManager() {
        _ctx = new Context();
        _threads = new ();
        _server = new (int.Parse(_ctx.Config["SOCKET_PORT"]));
    }

    ~ThreadManager() {
        foreach (var t in _threads) {
            t.Key.Cancel();
            _threads.TryRemove(t.Key, out _);
        }

        _server.Stop();
        _server.Dispose();
    }

    public async Task MainLoop() {
        _server.Start();
        _ctx.Logger.Log("Server started. Listening for clients...");

        while (true) {
            var client = await _server.AcceptTcpClientAsync();
            
            _ctx.Logger.Log($"Client connected: {client.Client.RemoteEndPoint}");
            
            var handler = new ServerTCP(client, _ctx);
            var thread = new Thread(StartBranch);
            
            _threads[new CancellationTokenSource()] = thread;
            
            thread.Start(handler);
        }
    }

    private void StartBranch(object? obj) {
        var client = (ServerTCP)(obj ?? throw new ArgumentException("Branch needs to have a client to manage"));
        var token = _threads.Last().Key.Token;
        client.Listen(token).Wait(token);
    }
}