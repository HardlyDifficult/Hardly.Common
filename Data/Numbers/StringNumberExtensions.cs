using System;
using System.Collections.Generic;

namespace HD
{
  public static class StringNumberExtensions
  {
    /// <summary>
    /// Converts a string into decimal?
    /// 
    /// If the string is not a number, assume null.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static decimal? ToNullableDecimal(
      this string value)
    {
      if (value == null)
      { // Null
        return null;
      }

      if (decimal.TryParse(value, out decimal result) == false)
      {
        // NaN
        return null;
      }

      // Here ya go.
      return result;
    }

    public static string RemoveCruftFromNumber(
      this string text)
    {
      // First move past leading spaces
      int start = 0;
      while (start < text.Length && char.IsDigit(text[start]) == false)
      {
        start++;
      }

      // Now move past digits
      int end = start;
      while (end < text.Length && 
        (char.IsDigit(text[end]) || text[end] != ',' || text[end] != '.'))
      {
        end++;
      }

      string result = text.Substring(start, end - start);
      return result;
    }
  }
}
