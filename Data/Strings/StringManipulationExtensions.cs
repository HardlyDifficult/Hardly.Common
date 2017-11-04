using System;
using System.Text;

namespace HD
{
  public static class StringManipulationExtensions
  {
    const int letterALowercase = 'a', letterZLowercase = 'z', letterAUppercase = 'A', letterZUppercase = 'Z', letterSpace = ' ', letter0 = '0', letter9 = '9';

    public static string[] AppendStrings(this object[] source, string[] stringsToAppend, string textBetween = null)
    {
      if (source != null && source.Length == stringsToAppend?.Length)
      {
        var mergedList = new string[source.Length];

        for (var i = 0; i < source.Length; i++)
        {
          if (i < stringsToAppend.Length)
          {
            mergedList[i] = source[i].ToString();
            if (textBetween != null)
            {
              mergedList[i] += textBetween;
            }
            mergedList[i] += stringsToAppend[i];
          }
          else
          {
            break;
          }
        }

        return mergedList;
      }

      return null;
    }

    public static string RemoveAll(this string source, char character)
    {
      return source.Replace(character.ToString(), "");
    }

    public static string RemoveSpecialChars(this string value, bool allowUnderscore)
    {
      for (var i = value.Length - 1; i >= 0; i--)
      {
        if (!value[i].IsNormalCharacter(true, true) && (!allowUnderscore || value[i] != '_'))
        {
          value = value.Remove(i, 1);
        }
      }

      return value;
    }

    public static byte[] ToBytesEncoded(this string data)
    {
      return Encoding.UTF8.GetBytes(data);
    }

    public static string ToCsv(this object[] values, string insertBetweenEach, string insertBeforeEachValue, string insertAfterEachValue)
    {
      var csv = "";

      for (var i = 0; i < values.Length; i++)
      {
        if (insertBetweenEach != null && i > 0)
        {
          csv += insertBetweenEach;
        }
        if (insertBeforeEachValue != null)
        {
          csv += insertBeforeEachValue;
        }

        csv += values[i];

        if (insertAfterEachValue != null)
        {
          csv += insertAfterEachValue;
        }
      }

      return csv;
    }

    public static string ToHtmlSafePlainText(this string value)
    {
      // TODO extend to cover all special chars, although this is enough for security
      return value?.Replace("<", "<wbr>&lt;").Replace(">", "><wbr>").Replace("=", "<wbr>=");
    }

    public static string ToHtmlSafeTagText(this string value)
    {
      // TODO extend to cover all special chars
      return value?.Replace("\"", "'");
    }

    public static string ToHtmlSafeUrl(this string value)
    {
      // TODO extend to cover all special chars
      return value?.Replace(" ", "%20");
    }

    public static string ToStringDecode(this byte[] data)
    {
      return Encoding.UTF8.GetString(data);
    }

    static bool IsNormalCharacter(this char character, bool includeNumbers, bool includeSpace)
    {
      var characterNumber = (int)character;
      if ((characterNumber >= letterALowercase && characterNumber <= letterZLowercase)
          || (characterNumber >= letterAUppercase && characterNumber <= letterZUppercase))
      {
        return true;
      }
      if (includeNumbers && (characterNumber >= letter0 && characterNumber <= letter9))
      {
        return true;
      }
      if (includeSpace && characterNumber == letterSpace)
      {
        return true;
      }

      return false;
    }
  }
}