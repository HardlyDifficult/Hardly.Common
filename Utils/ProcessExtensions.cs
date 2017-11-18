using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace HD
{

  public static class ProcessExtensions
  {
    /// <remarks>https://stackoverflow.com/a/268394</remarks>
    public static bool IsRunning(
      this Process process)
    {
      Debug.Assert(process != null);

      try
      {
        Process.GetProcessById(process.Id);
      }
      catch 
      {
        return false;
      }

      return true;
    }

    public static string GetInstanceName(
      this Process process)
    {
      string processName = Path.GetFileNameWithoutExtension(process.ProcessName);

      PerformanceCounterCategory cat = new PerformanceCounterCategory("Process");
      string[] instances = cat.GetInstanceNames()
          .Where(inst => inst.StartsWith(processName))
          .ToArray();

      foreach (string instance in instances)
      {
        using (PerformanceCounter cnt = new PerformanceCounter("Process",
            "ID Process", instance, true))
        {
          int val = (int)cnt.RawValue;
          if (val == process.Id)
          {
            return instance;
          }
        }
      }
      return null;
    }
  }
}