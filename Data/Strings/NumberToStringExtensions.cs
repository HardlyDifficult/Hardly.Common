using System;

namespace HD
{
  public static class NumberToString
  {
    #region A
    static readonly char[] AtoZLowercase
        = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
    static readonly char[] AtoZUppercase
        = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
    static readonly char[] Numbers
        = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
    #endregion A

    #region B
    static readonly char[] AtoZBothcases = AtoZLowercase.Append(AtoZUppercase);
    static readonly char[] AtoZBothcasesWithNumbers = AtoZBothcases.Append(Numbers);
    static readonly char[] AtoZLowercaseWithNumbers = AtoZLowercase.Append(Numbers);
    static char[] AtoZUppercaseWithNumbers = AtoZUppercase.Append(Numbers);
    #endregion B

    public static string ToRankString(this int i)
    {
      if (i % 10 == 1 && i % 100 != 11)
      {
        return $"{i}st";
      }
      if (i % 10 == 2 && i % 100 != 12)
      {
        return $"{i}nd";
      }
      if (i % 10 == 3 && i % 100 != 13)
      {
        return $"{i}rd";
      }
      return $"{i}th";
    }

    public static string ToStringAzViaMod(this uint i, bool includeUppercase, bool includeNumbers)
    {
      char[] availableChars;
      if (includeUppercase && includeNumbers)
      {
        availableChars = AtoZBothcasesWithNumbers;
      }
      else if (includeUppercase)
      {
        availableChars = AtoZBothcases;
      }
      else if (includeNumbers)
      {
        availableChars = AtoZLowercaseWithNumbers;
      }
      else
      {
        availableChars = AtoZLowercase;
      }

      return ((int)i).ToStringAzViaMod(availableChars);
    }

    public static string ToStringAzViaMod(this int i, char[] availableChars)
    {
      var value = "";

      if (i >= 0)
      {
        var mod = i % availableChars.Length;
        value += availableChars[mod];
        if (i >= availableChars.Length)
        {
          value += (i - availableChars.Length - mod).ToStringAzViaMod(availableChars);
        }
      }

      return value;
    }

    public static string ToStringWithCommaAndDecimals(this float value, uint numberOfFloats)
    {
      return ((double)value).ToStringWithCommaAndFloats(numberOfFloats);
    }

    public static string ToStringWithCommaAndFloats(this double value, uint numberOfFloats)
    {
      if ((long)value == value)
      {
        return value.ToString("N0");
      }
      return value.ToString("N" + numberOfFloats);
    }

    public static string ToStringWithCommas(this int value)
    {
      return value.ToString("N0");
    }

    public static string ToStringWithCommas(this uint value)
    {
      return value.ToString("N0");
    }

    public static string ToStringWithCommas(this long value)
    {
      return value.ToString("N0");
    }

    public static string ToStringWithCommas(this ulong value)
    {
      return value.ToString("N0");
    }
  }
}