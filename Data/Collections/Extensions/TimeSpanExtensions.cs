using System;

namespace HD
{
  public static class TimeSpanExtensions
  {
    public static string ToSimpleString(
      this TimeSpan time)
    {
      string message;

      if (time.Days > 0)
      {
        message = time.Days + " days";
        if (time.Hours > 2)
        {
          message += " " + time.Hours + " hours";
        }
      }
      else if (time.Hours > 0)
      {
        message = time.Hours + " hours";
        if (time.Minutes > 2)
        {
          message += " " + time.Minutes + " mins";
        }
      }
      else if (time.Minutes > 0)
      {
        message = time.Minutes + " mins";
        if (time.Seconds > 2)
        {
          message += " " + time.Seconds + " secs";
        }
      }
      else
      {
        if (time.Seconds > 2)
        {
          message = time.Seconds + " secs";
        }
        else
        {
          message = "soon";
        }
      }

      return message;
    }

    public static string ToShortTimeString(
      this TimeSpan timeSpan)
    {
      if (timeSpan.TotalMinutes <= 1)
      {
        int seconds = (int)Math.Round(timeSpan.TotalSeconds);
        return string.Format("{0} sec{1}", seconds, seconds > 1 ? "s" : "");
      }
      else if (timeSpan.TotalHours <= 1)
      {
        int mins = (int)timeSpan.TotalMinutes;
        int secs = timeSpan.Seconds;
        return string.Format("{0} min{1} {2} sec{3}", mins, mins > 1 ? "s" : "", secs, secs > 1 ? "s" : "");
      }
      else if (timeSpan.TotalDays <= 1)
      {
        int hours = (int)timeSpan.TotalHours;
        int mins = timeSpan.Minutes;
        return string.Format("{0} hour{1} {2} min{3}", hours, hours > 1 ? "s" : "", mins, mins > 1 ? "s" : "");
      }
      else
      {
        int days = (int)timeSpan.TotalDays;
        int hours = timeSpan.Hours;
        return string.Format("{0} day{1} {2} hour{3}", days, days > 1 ? "s" : "", hours, hours > 1 ? "s" : "");
      }
    }
  }
}
