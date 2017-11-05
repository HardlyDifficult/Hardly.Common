using System;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Threading;

namespace HD
{
  public class FileObserver
  {
    public event Action<string> observers;
    readonly string filename;
    string lastLine = null;
    Thread thread;
    DateTime lastLog;
    long currentPosition;

    public FileObserver(string filename)
    {
      this.filename = filename;

      //{
      //  // This pattern does not work when another process locks the file being observed.
        //FileSystemWatcher watcher = new FileSystemWatcher(Environment.CurrentDirectory, filename);
        //watcher.Changed += Watcher_Changed;
      //  watcher.EnableRaisingEvents = true;
      //}

      // Process everything found
    }

    //private void Watcher_Changed(object sender, FileSystemEventArgs e)
    //{
    //  Console.WriteLine("Changed");
    //}

    public void Start()
    {
      thread?.Abort();
      thread = new Thread(WatchFile_Blocking);
      thread.Start();
    }

    public void Stop()
    {
      thread?.Abort();
      thread = null;
    }

    void WatchFile_Blocking()
    {
      while (true)
      {
        try
        {
          using (FileStream fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
          {
            using (StreamReader streamReader = new StreamReader(fileStream))
            {
              // Skip any data already in the file
             // while (streamReader.ReadLine() != null) { }

              while (true)
              {
                string message;
                if ((message = streamReader.ReadLine()) != null)
                {
                  Observe(message);
                  currentPosition = fileStream.Position;
                  lastLog = DateTime.Now;
                }
                else
                {
                  if (DateTime.Now - lastLog > TimeSpan.FromSeconds(30))
                  {
                    break;
                  }
                  if (fileStream.Position > fileStream.Length)
                  { // Log file was cleared, move back to the beginning of the file
                    break;
                  }
                  Thread.Sleep(1000);
                }
              }
            }
          }
        }
        catch { }
        Thread.Sleep(1000);
      }
    }

    void Observe(string line)
    {
      Console.WriteLine("-------------->" + line);

      observers?.Invoke(line);
    }
  }
}
