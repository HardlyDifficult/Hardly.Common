using System;
using System.Net;
using System.Net.Sockets;

namespace HD
{
  public class TcpSocketClient : TcpSocket
  {
    public TcpSocketClient(
      int port)
    {
      TcpClient client = new TcpClient();
      try
      {
        client.Connect(new IPEndPoint(IPAddress.Loopback, port));
        OnConnection(client);
      }
      catch
      {
        // Failing here means the server is down, terminate process
        Environment.Exit(321);
      }
    }
  }
}
