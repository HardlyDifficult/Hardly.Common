using Common.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HD
{
  public class Throttle
  {
    static readonly ILog log = LogManager.GetLogger<Throttle>();

    readonly TimeSpan maxSlippage;

    DateTime nextRun;

    readonly TimeSpan minTimeBetweenRuns;

    public Throttle(
      TimeSpan minTimeBetweenRuns,
      TimeSpan maxSlippage)
    {
      nextRun = DateTime.MinValue;
      this.minTimeBetweenRuns = minTimeBetweenRuns;
      this.maxSlippage = maxSlippage;
    }

    /// <summary>
    /// TODO how-to deal with a huge backlog(?)
    /// </summary>
    public void SleepIfNeeded()
    {
      TimeSpan? timeToSleep = GetTimeToSleep();
      if (timeToSleep != null)
      {
        log.Info(timeToSleep.Value); // TODO trace;
        Thread.Sleep(timeToSleep.Value);
      }

      SetLastUpdateTime();
    }

    public async Task WaitTillReady()
    {
      TimeSpan? timeToSleep = GetTimeToSleep();
      if (timeToSleep != null)
      {
        log.Info(timeToSleep.Value); // TODO trace;
        await Task.Delay(timeToSleep.Value);
      }

      SetLastUpdateTime();
    }

    public void SetLastUpdateTime()
    {
      nextRun += minTimeBetweenRuns;

      DateTime now = DateTime.Now;
      if(now - nextRun > maxSlippage)
      {
        nextRun = now + minTimeBetweenRuns;
      }
    }

    TimeSpan? GetTimeToSleep()
    {
      DateTime now = DateTime.Now;
      if (nextRun > now)
      {
        return nextRun - now;
      }

      return null;
    }

    public void BackOff()
    {
      nextRun = DateTime.Now + TimeSpan.FromSeconds(10) + minTimeBetweenRuns;
    }
  }
}
