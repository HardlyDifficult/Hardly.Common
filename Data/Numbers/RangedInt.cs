using System;
using System.Diagnostics;

namespace HD
{
  /// <summary>
  /// A value which is capped to a range [0, max).
  /// When outside of this range, it loops around.  
  /// e.g. max = 3 then value 5 == 2.
  /// </summary>
  public struct RangedInt
  {
    #region Data
    /// <summary>
    /// We could refactor if we want to change the min of 0.
    /// </summary>
    const int minValue = 0;

    readonly int current;

    public readonly int maxExclusive;
    #endregion

    #region Properties
    int supportedValueRange
    {
      get
      {
        return maxExclusive - minValue;
      }
    }
    #endregion

    #region Init
    public RangedInt(
      int maxExclusive,
      int current = minValue)
    {
      Debug.Assert(current < maxExclusive);
      Debug.Assert(current >= minValue);

      this.maxExclusive = maxExclusive;
      this.current = current;
    }
    #endregion

    #region Operators
    public static RangedInt operator ++(
      RangedInt rangedInt)
    {
      rangedInt += 1;
      return rangedInt;
    }

    public static RangedInt operator --(
      RangedInt rangedInt)
    {
      rangedInt -= 1;
      return rangedInt;
    }

    public static RangedInt operator +(
      RangedInt rangedInt,
      int delta)
    {
      int newValue = rangedInt.current + delta;
      while (newValue >= rangedInt.maxExclusive)
      {
        newValue -= rangedInt.supportedValueRange;
      }
      while (newValue < minValue)
      {
        newValue += rangedInt.supportedValueRange;
      }
      return new RangedInt(rangedInt.maxExclusive, newValue);
    }

    public static RangedInt operator -(
      RangedInt rangedInt,
      int delta)
    {
      return rangedInt + (-delta);
    }

    public static explicit operator int(
      RangedInt myValue)
    {
      return myValue.current;
    }

    public static explicit operator uint(
      RangedInt myValue)
    {
      return (uint)myValue.current;
    }
    #endregion
  }
}