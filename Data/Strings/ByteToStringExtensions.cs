using System;

namespace HD
{
  public static class ByteToStringExtensions
  {
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