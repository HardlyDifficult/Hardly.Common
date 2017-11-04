using System;
using System.Collections.Generic;

namespace HD
{
  public static class Rng
  {
    #region Data
    static Random random = new Random();
    #endregion

    public static bool Bool(double percentChanceOfTrue = .5)
    {
      return random.NextDouble() < percentChanceOfTrue;
    }

    public static int RandomIndex<T>(this T[] array)
    {
      return random.Next(0, array.Length);
    }

    public static T RandomElement<T>(this T[] array)
    {
      return array[RandomIndex(array)];
    }

    public static double Double(double maxValue = 1)
    {
      return random.NextDouble() * maxValue;
    }

    public static uint Uint(int maxExclusive = int.MaxValue)
    {
      return (uint)random.Next(maxExclusive);
    }

    public static uint Uint(uint min, uint maxExclusive)
    {
      return (uint)random.Next((int)min, (int)maxExclusive);
    }

    public static int Int(int maxExclusive = int.MaxValue)
    {
      return random.Next(maxExclusive);
    }

    public static int Int(int min, int maxExclusive)
    {
      return random.Next(min, maxExclusive);
    }

    public static void SetSeed(int seed)
    {
      random = new Random(seed);
    }

    public static float Float(float max = float.MaxValue)
    {
      return (float)random.NextDouble() * max;
    }

    public static float Float(float min, float max)
    {
      return (float)random.NextDouble() * (max - min) + min;
    }

    public static T Enum<T>(Type type)
    {
      var array = System.Enum.GetValues(type);
      var index = Uint(array.Length);
      return (T)array.GetValue(index);
    }
  }
}
