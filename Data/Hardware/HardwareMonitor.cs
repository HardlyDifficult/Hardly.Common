using System;
using System.Diagnostics;

namespace HD
{
  public static class HardwareMonitor
  {
    const int numberOfSamples = 20;

    readonly static RollingHistory totalCpuRollingHistory = new RollingHistory(numberOfSamples);
    readonly static RollingHistory miningCpuRollingHistory = new RollingHistory(numberOfSamples);

    static readonly PerformanceCounter total_cpu = new PerformanceCounter("Processor", "% Processor Time", "_Total");

    public static PerformanceCounter minerProcessPerformanceCounter
    {
      private get; set;
    }

    public static double percentTotalCPU
    {
      get
      {
        return totalCpuRollingHistory.value;
      }
    }

    public static double percentMinerCPU
    {
      get
      {
        return miningCpuRollingHistory.value;
      }
    }

    public static void RefreshValues()
    {
      totalCpuRollingHistory.Add(total_cpu.NextValue());
      if (minerProcessPerformanceCounter != null)
      {
        miningCpuRollingHistory.Add(minerProcessPerformanceCounter.NextValue() / Environment.ProcessorCount);
      }
      else
      {
        miningCpuRollingHistory.Clear();
      }
    }
  }
}
