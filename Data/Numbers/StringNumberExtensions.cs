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
  }
}
