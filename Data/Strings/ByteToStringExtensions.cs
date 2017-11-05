using System;
using System.Text;

namespace HD
{
  public static class ByteToStringExtensions
  {
    public static string ToHexString(
     this byte[] valueList)
    {
      StringBuilder hex = new StringBuilder(valueList.Length * 2);
      for (int i = 0; i < valueList.Length; i++)
      {
        hex.AppendFormat("{0:x2}", valueList[i]);
      }

      return hex.ToString();
    }

    public static string GetNextLine(
      this byte[] data, 
      ref uint index)
    {
      if (index >= data.Length)
      {
        return null;
      }

      var line = "";

      do
      {
        var c = (char)data[index];
        if (c.Equals('\r'))
        {
          // do nothing, wait for \n
        }
        else if (c.Equals('\n'))
        {
          index++;
          break;
        }
        else
        {
          line += c;
        }
        index++;
      } while (index < data.Length);

      return line;
    }
  }
}