using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;

namespace HD
{
  public class TcpSocketServer : TcpSocket
  {
    readonly TcpListener serverListener;

    public TcpSocketServer(
      int port)
    {
      Debug.Assert(port >= 1);
      Debug.Assert(port <= 65535);

      serverListener = new TcpListener(IPAddress.Loopback, port);
      serverListener.Start();
      BeginConnection();
    }
    
    /// <summary>
    /// Async establish connection.
    /// Once a connection is formed, start listening for another.
    /// </summary>
    void BeginConnection()
    {
      serverListener.BeginAcceptTcpClient((ar) =>
      {
        OnConnection(serverListener.EndAcceptTcpClient(ar));
        BeginConnection();
      }, null);
    }
  }
}
