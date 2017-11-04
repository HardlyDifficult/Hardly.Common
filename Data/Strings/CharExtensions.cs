using System;

namespace HD
{
  public static class CharExtensions
  {
    public static string Repeat(
      this char c, 
      uint count)
    {
      return new string(c, (int)count);
    }
  }
}