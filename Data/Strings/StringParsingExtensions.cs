using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace HD
{
  public static class StringParsing
  {
    #region Constants
    static readonly string[] dayOfWeek = new string[]
    {
      "sun",
      "mon",
      "tue",
      "wed",
      "thu",
      "fri",
      "sat",
    };
    #endregion

    public static string GetAfter(
      this string source,
      string searchToken,
      bool isFirstNotLastInstance = true)
    {
      if (source != null)
      {
        if (!string.IsNullOrEmpty(searchToken))
        {
          int i;
          if (isFirstNotLastInstance)
          {
            i = source.IndexOf(searchToken, StringComparison.CurrentCulture);
          }
          else
          {
            i = source.LastIndexOf(searchToken, StringComparison.CurrentCulture);
          }
          if (i >= 0)
          {
            i += searchToken.Length;
            if (i < source.Length)
            {
              return source.Substring(i);
            }
            if (i == source.Length)
            {
              return "";
            }
          }

          return null;
        }

        Debug.Fail("String get after");
      }
      return null;
    }

    public static string GetBefore(
      this string source,
      string searchToken,
      bool firstOrLastInstance = true)
    {
      if (source != null)
      {
        if (!string.IsNullOrEmpty(searchToken))
        {
          int i;
          if (firstOrLastInstance)
          {
            i = source.IndexOf(searchToken, StringComparison.CurrentCulture);
          }
          else
          {
            i = source.LastIndexOf(searchToken, StringComparison.CurrentCulture);
          }
          if (i >= 0)
          {
            if (i < source.Length)
            {
              return source.Substring(0, i);
            }
          }
          else
          {
            return source;
          }
        }
        else
        {
          Debug.Fail("string get before");
        }
      }

      return null;
    }

    public static string GetBetween(
      this string source, 
      string searchToken1, 
      string searchToken2, 
      bool firstOrLastInstance1 = true, 
      bool firstOrLastInstance2 = true,
      bool requireEndToken = false)
    {
      if (source != null)
      {
        if (!string.IsNullOrEmpty(searchToken1) && !string.IsNullOrEmpty(searchToken2))
        {
          int iStart;
          if (firstOrLastInstance1)
          {
            iStart = source.IndexOf(searchToken1, StringComparison.CurrentCulture);
          }
          else
          {
            iStart = source.LastIndexOf(searchToken1, StringComparison.CurrentCulture);
          }
          if (iStart >= 0)
          {
            iStart += searchToken1.Length;
            int iEnd;
            if (firstOrLastInstance2)
            {
              iEnd = source.IndexOf(searchToken2, iStart, StringComparison.CurrentCulture);
            }
            else
            {
              iEnd = source.LastIndexOf(searchToken2, StringComparison.CurrentCulture);
            }

            if(iEnd < 0 && requireEndToken == false)
            {
              iEnd = source.Length;
            }

            if (iEnd > iStart)
            {
              return source.Substring(iStart, iEnd - iStart);
            }
            if (iEnd == iStart)
            {
              return "";
            }
          }
        }
        else
        {
          Debug.Fail("string get between");
        }
      }

      return null;
    }

    public static bool IsEmpty(this string value)
    {
      return string.IsNullOrEmpty(value);
    }

    public static bool IsLowercase(this string value)
    {
      return value.Trim().Equals(value);
    }

    public static bool IsTrimmed(this string value)
    {
      return value.IsEmpty() || value.Trim().Equals(value);
    }

    public static string[] Tokenize(this string source, string token, bool includeToken = false)
    {
      if (!string.IsNullOrEmpty(token))
      {
        List<string> results = new List<string>();

        while (true)
        {
          var result = GetBefore(source, token);
          if (result == null)
          {
            break;
          }
          if (result.Length > 0)
          {
            if (includeToken)
            {
              result += token;
            }
            results.Add(result);
          }
          source = GetAfter(source, token);
        }

        if (includeToken && source.StartsWith(token))
        {
          source += token;
        }
        results.Add(source);

        return results.ToArray();
      }
      Debug.Fail("string tokenize");

      return null;
    }

    /// <summary>
    /// day is today if this fails.
    /// </summary>
    public static bool TryGetDayOfWeek(
     this string dayString,
     out int day)
    {
      DateTime now = DateTime.Now;

      day = now.Day;
      if (string.IsNullOrEmpty(dayString) == false)
      {
        for (int dayIndex = 0; dayIndex < dayOfWeek.Length; dayIndex++)
        {
          if (dayString.IndexOf(dayOfWeek[dayIndex], StringComparison.InvariantCultureIgnoreCase) >= 0)
          {
            day += dayIndex - (int)now.DayOfWeek;
            return true;
          }
        }
      }

      return false;
    }
  }
}