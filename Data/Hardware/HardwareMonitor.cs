using System;
using System.Diagnostics;

namespace HD
{
  // TODO this is not ready to be in HardlyCommon..
  public static class HardwareMonitor
  {
    const int numberOfSamples = 20;

    readonly static RollingHistory totalCpuRollingHistory = new RollingHistory(numberOfSamples);

    readonly static RollingHistory miningCpuRollingHistory = new RollingHistory(numberOfSamples);

    /// <summary>
    /// This appears to return results 10% different than the Windows Task Manager.... ?
    /// </summary>
    static readonly PerformanceCounter total_cpu 
      = new PerformanceCounter("Processor", "% Processor Time", "_Total");

    public static PerformanceCounter minerProcessPerformanceCounter
    {
      private get; set;
    }

    public static double percentTotalCPU
    {
      get
      {
        return totalCpuRollingHistory.value / 100;
      }
    }

    public static double percentMinerCPU
    {
      get
      {
        return miningCpuRollingHistory.value / 100;
      }
    }

    public static void RefreshValues()
    {
      totalCpuRollingHistory.Add(total_cpu.NextValue());
      if (minerProcessPerformanceCounter != null)
      {
        // TODO why doesn't this work in release mode only
        miningCpuRollingHistory.Add(minerProcessPerformanceCounter.NextValue() / Environment.ProcessorCount);
      }
      else
      {
        miningCpuRollingHistory.Clear();
      }
    }
  }
}
