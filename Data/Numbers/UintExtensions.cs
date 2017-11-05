using System;
using System.Diagnostics;

namespace HD
{
  public static class UintExtensions
  {
    public unsafe static void GetBytes(
      this uint value,
      byte[] output,
      int outputStartingIndex = 0)
    {
      Debug.Assert(output.Length - outputStartingIndex >= 4);
      fixed (byte* b = output)
      {
        *((uint*)(b + outputStartingIndex)) = value;
      }
    }
  }
}
