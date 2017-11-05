using System;
using System.Diagnostics;

namespace HD
{
  public static class UlongExtensions
  {
    public unsafe static void GetBytes(
      this ulong value,
      byte[] output,
      int outputStartingIndex = 0)
    {
      Debug.Assert(output.Length - outputStartingIndex >= 8);
      fixed (byte* b = output)
      {
        *((ulong*)(b + outputStartingIndex)) = value;
      }
    }
    /// 64 x 64 -> 128 unsigned multiply.
    /// in c++, we could use compiler intrinsics, as this is a single instruction on x64
    /// without that, the best we can do is to expand it to 4 32x32 muls
    public static void UnsignedMultiply128(
      this ulong a,
      ulong b,
      out ulong resultLow,
      out ulong resultHigh)
    {
      ulong aLow  = a & 0xffffffffU;
      ulong aHigh = a >> 32;
      ulong bLow  = b & 0xffffffffU;
      ulong bHigh = b >> 32;

      ulong axbHigh = aHigh * bHigh;
      ulong axbMid  = aHigh * bLow ;
      ulong bxaMid  = bHigh * aLow ;
      ulong axbLow  = aLow  * bLow ;

      ulong midLow = (axbMid & 0xffffffffU) + (bxaMid & 0xffffffffU);
      ulong midHigh = (axbMid >> 32) + (bxaMid >> 32);
      ulong carryBit = (midLow + (axbLow >> 32)) >> 32;
      unchecked { resultLow = axbLow + (midLow << 32); }
      resultHigh = axbHigh + (axbMid >> 32) + (bxaMid >> 32) + carryBit;
    }
  }
}
