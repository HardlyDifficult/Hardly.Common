using HD;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace HD
{
  public sealed class AutoUpdateWithThrottle
  {
    readonly Func<Task> onRefresh;

    readonly Throttle throttle;

    readonly System.Timers.Timer timerRefreshData;

    readonly CancellationToken cancellationToken;

    public AutoUpdateWithThrottle(
      Func<Task> onRefresh,
      TimeSpan timeBetweenRefresh,
      Throttle throttle,
      CancellationToken cancellationToken)
    {
      Debug.Assert(onRefresh != null);
      Debug.Assert(throttle != null);

      this.onRefresh = onRefresh;
      this.throttle = throttle;
      this.cancellationToken = cancellationToken;

      // TODO
      // Set the throttle to half the stated max requests per min
      //throttle = new Throttle(TimeSpan.FromMilliseconds(
      //  2 * TimeSpan.FromMinutes(1).TotalMilliseconds / maxRequestsPerMinute));

      timerRefreshData = new System.Timers.Timer(timeBetweenRefresh.TotalMilliseconds)
      {
        AutoReset = false,
      };
      timerRefreshData.Elapsed += Timer_Elapsed;
    }

    /// <summary>
    /// Start auto-updates with the first update happening
    /// after timeBetweenRefresh has passed.
    /// </summary>
    public void Start()
    {
      Debug.Assert(timerRefreshData.Enabled == false);

      timerRefreshData.Start();
    }

    /// <summary>
    /// Requests the first update now (before returning)
    /// and starts the timer for auto-updates.
    /// </summary>
    /// <returns></returns>
    public async Task StartWithImmediateResults()
    {
      await Refresh();
    }

    async void Timer_Elapsed(
      object sender,
      ElapsedEventArgs e)
    {
      await Refresh();
    }

    async Task Refresh()
    {
      await throttle.WaitTillReady();
      await onRefresh?.Invoke();
      throttle.SetLastUpdateTime();

      if (cancellationToken.IsCancellationRequested == false)
      { // Refresh until cancelled.
        timerRefreshData.Start();
      }
    }
  }
}