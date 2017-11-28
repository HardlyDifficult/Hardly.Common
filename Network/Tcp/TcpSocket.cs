using System;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace HD
{
  /// <summary>
  /// Manages a single Tcp client or server connection
  /// 
  /// TODO we should support multiple clients.
  /// </summary>
  public class TcpSocket
  {
    #region Constants
    const string
      ping = "Ping",
      pong = "Pong";
    #endregion

    #region Data
    public event Action<string> onMessage;

    public event Action onConnection;

    public event Action<Exception> onDisconnect;

    TcpClient client;

    StreamReader reader;

    StreamWriter writer;

    readonly AutoResetEvent pingEvent = new AutoResetEvent(false);
    #endregion

    #region Events
    protected void OnConnection(
      TcpClient client)
    {
      Debug.Assert(client != null);

      this.client = client;

      Stream stream = client.GetStream();
      Debug.Assert(stream != null);

      reader = new StreamReader(stream);
      writer = new StreamWriter(stream);

      ReadLoopAsync();

      onConnection?.Invoke();
    }
    #endregion

    #region Write
    public bool Ping()
    {
      Send(ping);
      if (pingEvent.WaitOne(50))
      {
        return true;
      }

      return false;
    }

    public void Send(
      string message)
    {
      try
      {
        writer.WriteLine(message);
        writer.Flush();
      }
      catch (Exception e)
      {
        try
        {
          client.Close();
        }
        catch { }
        onDisconnect?.Invoke(e);
      }
    }
    #endregion

    #region Private
    async void ReadLoopAsync()
    {
      try
      {
        while (true)
        {
          string message = await reader.ReadLineAsync();
          if (message == null)
          { // Disconnected
            break;
          }

          if (message == ping)
          {
            Send(pong);
            return;
          }
          else if (message == pong)
          {
            pingEvent.Set();
            return;
          }

          onMessage(message);
        }
      }
      catch (Exception e)
      {
        try
        {
          client.Close();
        }
        catch { }

        onDisconnect?.Invoke(e);
      }

      try
      {
        client.Close();
      }
      catch { }
      onDisconnect?.Invoke(null);
    }
    #endregion
  }
}
