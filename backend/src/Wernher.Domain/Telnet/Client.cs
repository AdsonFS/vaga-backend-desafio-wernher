using System.Net.Sockets;

namespace Wernher.Domain.Telnet;

public class Client : IDisposable
{
    private readonly TcpClient _tcpClient;
    private readonly NetworkStream _networkStream;
    private readonly StreamReader _streamReader;
    private readonly StreamWriter _streamWriter;
    public Client(string host)
    {
        var hostParts = host.Split(':');
        var Host = hostParts[0];

        var Port = hostParts.Length == 2 ? int.Parse(hostParts[1]) : 23;
        System.Console.WriteLine($"Connecting to {Host}:{Port}");

        _tcpClient = new TcpClient(Host, Port);


        _networkStream = _tcpClient.GetStream();
        _streamReader = new StreamReader(_networkStream);
        _streamWriter = new StreamWriter(_networkStream) { AutoFlush = true };
    }
    public async Task<string> GetDataAsync(string command)
    {
        // discart the initial message if there is any
        await _streamReader.ReadLineAsync();

        await _streamWriter.WriteLineAsync(command);
        return await _streamReader.ReadLineAsync();
    }

    public async Task CloseTelnetAsync()
    {
        await _streamWriter.WriteLineAsync("quit\r\n");
        Dispose();
    }

    public void Dispose()
    {
        _streamReader.Dispose();
        _streamWriter.Dispose();
        _networkStream.Dispose();
        _tcpClient.Dispose();
    }
}