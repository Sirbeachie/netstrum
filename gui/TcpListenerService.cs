using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace netstrum_gui
{
    public class TcpListenerService
    {
        private readonly TcpListener _listener;
        private CancellationTokenSource? _cts;
        public event Action<string>? PacketReceived;

        public TcpListenerService(int port)
        {
            _listener = new TcpListener(IPAddress.Loopback, port);
        }

        public async Task StartAsync()
        {
            _cts = new CancellationTokenSource();
            _listener.Start();
            try
            {
                while (!_cts.Token.IsCancellationRequested)
                {
                    var client = await _listener.AcceptTcpClientAsync(_cts.Token);
                    _ = HandleClientAsync(client, _cts.Token);
                }
            }
            catch (OperationCanceledException)
            {
            }
        }

        private async Task HandleClientAsync(TcpClient client, CancellationToken token)
        {
            using var reader = new StreamReader(client.GetStream(), Encoding.UTF8);
            while (!token.IsCancellationRequested && !reader.EndOfStream)
            {
                var line = await reader.ReadLineAsync();
                if (line != null)
                {
                    PacketReceived?.Invoke(line);
                }
            }
        }

        public void Stop()
        {
            _cts?.Cancel();
            _listener.Stop();
        }
    }
}
