using System;
using System.Diagnostics;

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
      catch (ArgumentException)
      {
        return false;
      }

      return true;
    }
  }
}