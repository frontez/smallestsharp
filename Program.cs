using System.Net;
using System.Net.Sockets;

class Program
{
    private static readonly byte[] HttpResponse200 =
        "HTTP/1.1 200 OK\r\n"u8 +
        "Content-Type: text/plain\r\n"u8 +
        "Content-Length: 11\r\n"u8 +
        "Connection: close\r\n"u8 +
        "\r\n"u8 +
        "hello world"u8;

    private static readonly byte[] GetRoot = "GET / "u8;
    private static readonly byte[] GetHello = "GET /hello "u8;

    static void Main()
    {
        using var listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        listener.Bind(new IPEndPoint(IPAddress.Any, 8080));
        listener.Listen();

        var buffer = new byte[128];

        while (true)
        {
            using var client = listener.Accept();

            var bytesRead = client.Receive(buffer, SocketFlags.None);
            var requestSpan = new Span<byte>(buffer, 0, bytesRead);

            if (requestSpan.StartsWith(GetRoot) || requestSpan.StartsWith(GetHello))
            {
                client.Send(HttpResponse200, SocketFlags.None);
            }
        }
    }
}
