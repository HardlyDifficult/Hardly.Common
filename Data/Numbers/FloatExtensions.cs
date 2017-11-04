using System;

namespace HD
{
  public static class FloatExtensions
  {
    public static bool IsApprox(
      this float a, 
      float b)
    {
      return HDMath.Abs(a - b) < 0.0001f;
    }

    public static bool IsReal(this float value)
    {
      return !float.IsNaN(value) && !float.IsInfinity(value);
    }

    public static bool IsZero(this float value)
    {
      return !(value > 0 || value < 0);
    }

    public static float Lerp(this float from, float target, float percent)
    {
      if (percent >= 1)
      {
        return target;
      }
      return (target - from) * percent + from;
    }

    public static float Round128(this float value)
    {
      return HDMath.Round(value * 128f) * (1f / 128f);
    }
  }
}