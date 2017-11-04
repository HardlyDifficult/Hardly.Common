using System;
using System.Collections.Generic;

namespace HD
{
  public static class StringExtensions
  {
    public static void ToByteArrayFromHex(
      this string hexString,
      byte[] output)
    {
      for (int index = 0; index < output.Length; index++)
      {
        string byteValue = hexString.Substring(index * 2, 2);
        output[index] = Convert.ToByte(byteValue, 16);
      }
    }

    public static string Reverse(this string s)
    {
      var charArray = s.ToCharArray();
      Array.Reverse(charArray);
      return new string(charArray);
    }

    public static List<string> Split(
      this string s, 
      char splitChar, 
      uint maxSizeOfResultArray)
    {
      if (s == null)
      {
        return null;
      }

      var resultList = new List<string>();
      var lastI = 0;
      for (int i = 0; i < s.Length && resultList.Count < maxSizeOfResultArray - 1; i++)
      {
        if (s[i] == splitChar)
        {
          if (i - lastI == 0)
          {
            continue;
          }

          resultList.Add(s.Substring(lastI, i - lastI));
          lastI = i + 1;
        }
      }

      if (lastI < s.Length)
      {
        resultList.Add(s.Substring(lastI));
      }

      return resultList;
    }
  }
}