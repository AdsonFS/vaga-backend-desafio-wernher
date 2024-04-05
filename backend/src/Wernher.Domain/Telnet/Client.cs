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

        var Port = hostParts.Length == 2 ? int.Parse(hostParts[1]) : 2023;
        System.Console.WriteLine($"Connecting to {Host}:{Port}");

        _tcpClient = new TcpClient(Host, Port);
        _networkStream = _tcpClient.GetStream();
        _streamReader = new StreamReader(_networkStream);
        _streamWriter = new StreamWriter(_networkStream) { AutoFlush = true };
    }

    public async Task<string> GetData(string command)
    {
        // clean buffer before sending command
        await _streamWriter.WriteLineAsync(command);
        await _streamReader.ReadLineAsync();

        // send command
        await _streamWriter.WriteLineAsync(command);
        var response = await _streamReader.ReadLineAsync();

        return response;
    }

    public async Task CloseTelnet()
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