using System;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

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

    TcpClient _client;

    StreamWriter writer;

    readonly AutoResetEvent pingEvent = new AutoResetEvent(false);

    object myLock = new object();

    Thread thread;
    #endregion

    #region Properties
    TcpClient client
    {
      get
      {
        return _client;
      }
      set
      {
        lock (myLock)
        {

          if (value == null)
          {
            try
            {
              thread?.Abort();
            }
            catch { }

            try
            {
              client.Close();
            }
            catch { }
          }
          _client = value;
        }
      }
    }
    #endregion

    #region Events
    protected void OnConnection(
      TcpClient client)
    {
      Debug.Assert(client != null);

      this.client = client;

      Stream stream = client.GetStream();
      Debug.Assert(stream != null);

      writer = new StreamWriter(stream);

      onConnection?.Invoke();

      try
      {
        thread?.Abort();
      }
      catch { }
      thread = new Thread(Run);
      thread.Start();
    }
    #endregion

    #region Write
    protected virtual void Run()
    {
      try
      {
        StreamReader reader = new StreamReader(client.GetStream());
        while (true)
        {
          string message = reader.ReadLine();
          if (message == null)
          { // Disconnected
            break;
          }

          if (message == ping)
          {
            Send(pong);
            continue;
          }
          else if (message == pong)
          {
            pingEvent.Set();
            continue;
          }

          onMessage(message);
        }
      }
      catch (Exception e)
      {
        client = null;
        onDisconnect?.Invoke(e);
        return;
      }

      client = null;
      onDisconnect?.Invoke(null);
    }

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
      if (client == null)
      {
        return;
      }

      try
      {
        writer.WriteLine(message);
        writer.Flush();
      }
      catch (Exception e)
      {
        client = null;
        onDisconnect?.Invoke(e);
      }
    }
    #endregion
  }
}
