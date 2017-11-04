using System;
using System.Diagnostics;

namespace HD
{
  public static class HDMath
  {
    public const float Deg2Rad = PI / 180f;
    public const float deg360InRadians = PI * 2, deg90InRadians = PI / 2, deg180InRadians = PI,
                deg270InRadians = deg360InRadians - deg90InRadians, deg45InRadians = PI / 4;
    public const float E = 2.71828182845904523536028747135266249775724709369995f;
    public const float PI = 3.141592653589793238462643383279502884197169399375105820974944592307816406286208998628f;
    public const float Rad2Deg = 180f / PI;

    public static int Abs(int a)
    {
      if (a < 0)
      {
        return a * -1;
      }
      return a;
    }

    public static float Abs(float a)
    {
      if (a < 0)
      {
        return a * -1;
      }
      return a;
    }

    public static double Abs(double a)
    {
      if (a < 0)
      {
        return a * -1;
      }
      return a;
    }

    //public const float PIm = 3.141592653589793238462643383279502884197169399375105820974944592307816406286208998628f;
    //public const float Deg2Radm = PIm / 180f;
    //public static Position Abs(Position a) {
    //    if(a < 0) {
    //        return a * -1;
    //    } else {
    //        return a;
    //    }
    //}
    //public static float Abs(float a) {
    //    if(a < 0) {
    //        return a * -1;
    //    } else {
    //        return a;
    //    }
    //}
    public static int Ceiling(float a)
    {
      return (int)System.Math.Ceiling(a);
    }

    public static double Exp(double a)
    {
      return System.Math.Exp(a);
    }

    public static int Floor(float a)
    {
      return (int)System.Math.Floor(a);
    }

    public static int Floor(double a)
    {
      return (int)System.Math.Floor(a);
    }

    public static float Lerp(float from, float to, float percent)
    {
      var value = to - from;
      value *= percent;
      value += from;
      return value;
    }

    public static int Log(int a)
    {
      return (int)System.Math.Log(a);
    }

    public static float Log(float a)
    {
      return (float)System.Math.Log(a);
    }

    public static double Log(double a)
    {
      return System.Math.Log(a);
    }

    public static ulong Max(ulong a, ulong b)
    {
      if (a > b)
      {
        return a;
      }
      return b;
    }
    public static long Max(long a, long b)
    {
      if (a > b)
      {
        return a;
      }
      return b;
    }

    public static decimal Max(decimal a, decimal b)
    {
      if (a > b)
      {
        return a;
      }
      return b;
    }

    //public static Position Max(Position a, Position b) {
    //    if(a > b) {
    //        return a;
    //    } else {
    //        return b;
    //    }
    //}
    public static uint Max(uint a, uint b)
    {
      if (a > b)
      {
        return a;
      }
      return b;
    }

    //public static Position Max(Position a, Position b, Position c) {
    //    if(a > b) {
    //        if(a > c) {
    //            return a;
    //        } else {
    //            return c;
    //        }
    //    } else {
    //        if(b > c) {
    //            return b;
    //        } else {
    //            return c;
    //        }
    //    }
    //}
    public static int Max(int a, int b)
    {
      if (a > b)
      {
        return a;
      }
      return b;
    }

    public static float Max(float a, float b, float c)
    {
      if (a > b)
      {
        if (a > c)
        {
          return a;
        }
        return c;
      }
      if (b > c)
      {
        return b;
      }
      return c;
    }

    public static float Max(float a, float b, float c, float d)
    {
      if (a > b)
      {
        if (a > c)
        {
          if (a > d)
          {
            return a;
          }
          return d;
        }
        if (c > d)
        {
          return c;
        }
        return d;
      }
      if (b > c)
      {
        if (b > d)
        {
          return b;
        }
        return d;
      }
      if (c > d)
      {
        return c;
      }
      return d;
    }

    public static float Max(float a, float b)
    {
      if (a > b)
      {
        return a;
      }
      return b;
    }

    //public static float Max(float a, float b) {
    //    if(a > b) {
    //        return a;
    //    } else {
    //        return b;
    //    }
    //}
    public static double Max(double a, double b)
    {
      if (a > b)
      {
        return a;
      }
      return b;
    }

    public static ulong Min(ulong a, ulong b)
    {
      if (a < b)
      {
        return a;
      }
      return b;
    }

    //public static Position Min(Position a, Position b) {
    //    if(a < b) {
    //        return a;
    //    } else {
    //        return b;
    //    }
    //}
    public static uint Min(uint a, uint b)
    {
      if (a < b)
      {
        return a;
      }
      return b;
    }

    public static int Min(int a, int b)
    {
      if (a < b)
      {
        return a;
      }
      return b;
    }

    public static float Min(float a, float b, float c, float d)
    {
      if (a < b)
      {
        if (a < c)
        {
          if (a < d)
          {
            return a;
          }
          return d;
        }
        if (c < d)
        {
          return c;
        }
        return d;
      }
      if (b < c)
      {
        if (b < d)
        {
          return b;
        }
        return d;
      }
      if (c < d)
      {
        return c;
      }
      return d;
    }

    public static float Min(float a, float b)
    {
      if (a < b)
      {
        return a;
      }
      return b;
    }

    public static double Min(double a, double b)
    {
      if (a < b)
      {
        return a;
      }
      return b;
    }

    //public static float Min(float a, float b) {
    //    if(a < b) {
    //        return a;
    //    } else {
    //        return b;
    //    }
    //}
    public static float Pow(float a, int exp)
    {
      for (var i = 0; i < exp; i++)
      {
        a *= a;
      }

      return a;
    }

    public static int Pow(int a, int exp)
    {
      return (int)System.Math.Pow(a, exp);
    }

    public static double Pow(double a, double exp)
    {
      return System.Math.Pow(a, exp);
    }

    public static float Pow(float a, float exp)
    {
      return (float)System.Math.Pow(a, exp);
    }

    public static int Range(
      int min,
      int max,
      int value)
    {
      Debug.Assert(min <= max);
      return Max(min, Min(max, value));
    }

    public static uint Range(uint min, uint max, uint value)
    {
      Debug.Assert(min <= max);
      return Max(min, Min(max, value));
    }

    public static float Range(float min, float max, float value)
    {
      Debug.Assert(min <= max);
      return Max(min, Min(max, value));
    }

    public static double Range(double min, double max, double value)
    {
      Debug.Assert(min <= max);
      return Max(min, Min(max, value));
    }

    //public static float Pow(float a, int exp) {
    //    return Math.Pow(a, exp);
    //}
    public static int Round(float a)
    {
      return (int)System.Math.Round(a);
    }

    public static float Round(float a, int numberOfDecimals)
    {
      return (float)System.Math.Round(a, numberOfDecimals);
    }

    public static uint Sqrt(uint a)
    {
      return (uint)System.Math.Sqrt(a);
    }

    //public static Position Sqrt(Position a) {
    //    return System.Math.Sqrt((double)a);
    //}
    public static int Sqrt(int a)
    {
      return (int)System.Math.Sqrt(a);
    }

    public static float Sqrt(float a)
    {
      return (float)System.Math.Sqrt(a);
    }

    public static double Sqrt(double a)
    {
      return System.Math.Sqrt(a);
    }

    internal static double Log10(int value)
    {
      if (value == 0)
      {
        return 0;
      }
      return System.Math.Log10(value);
    }

    //public static float Sqrt(float a) {
    //    //return (float)System.Math.Sqrt((double)a);

    //    var n = a / 2.0f;
    //    var lstX = 0.0f;
    //    while(n != lstX) {
    //        lstX = n;
    //        n = (n + a / n) / 2.0f;
    //    }
    //    return n;
    //}

    //public static float Sqrt(float a) {
    //    return (float)System.Math.Sqrt(a);
    //}

    #region Trig

    //public static float Cos(float theta) {
    //    return (float)System.Math.Cos((double)theta);

    //    //var sign = 1;

    //    //while(theta < 0) {
    //    //    theta += deg360InRadians;
    //    //}
    //    //while(theta > deg360InRadians) {
    //    //    theta -= deg360InRadians;
    //    //}

    //    //if(theta >= deg270InRadians) {
    //    //    theta = deg360InRadians - theta;
    //    //} else if(theta >= deg180InRadians) {
    //    //    theta = theta - deg180InRadians;
    //    //    sign = -1;
    //    //} else if(theta >= deg90InRadians) {
    //    //    theta = deg180InRadians - theta;
    //    //    sign = -1;
    //    //}

    //    //if(theta >= deg45InRadians) {
    //    //    return sign * Sin(deg90InRadians - theta);
    //    //} else {
    //    //    return sign * (1 - Pow(theta, 2) / 2 + Pow(theta, 4) / 24 - Pow(theta, 6) / 720 + - +);
    //    //}
    //}

    public static float ACos(float theta)
    {
      return (float)System.Math.Acos(theta);
    }

    //public static float ACos(float theta) {
    //    return ATan(Sqrt(1 - Pow(theta, 2)) / theta);
    //}
    public static double ACos(double theta)
    {
      return System.Math.Acos(theta);
    }

    public static float ASin(float theta)
    {
      return (float)System.Math.Asin(theta);
    }

    //public static float ASin(float theta) {
    //    return ATan(theta / Sqrt(1 - Pow(theta, 2)));
    //}
    public static double ASin(double theta)
    {
      return System.Math.Asin(theta);
    }

    public static float ATan(float theta)
    {
      return (float)System.Math.Atan(theta);
    }

    //    return sign * (theta - Pow(theta, 3) / 3 + Pow(theta, 5) / 5);
    //}
    public static double ATan(double theta)
    {
      return System.Math.Atan(theta);
    }

    public static float ATan2(float y, float x)
    {
      return (float)System.Math.Atan2(y, x);
    }

    //    if(theta > 0.26794919243f) {
    //        sign *= PIm / 6;
    //        theta = (SqrtOf3f * theta - 1) / (SqrtOf3f + theta);
    //    }
    //public static float ATan2(float y, float x) {
    //    if(x > 0) {
    //        return ATan(y / x);
    //    } else if(x < 0) {
    //        if(y >= 0) {
    //            return ATan(y / x) + PIm;
    //        } else {
    //            return ATan(y / x) - PIm;
    //        }
    //    } else if(y > 0) {
    //        return PIm / 2;
    //    } else if(y < 0) {
    //        return -PIm / 2;
    //    } else {
    //        Debug.Fail("Bad atan request");
    //        return 0;
    //    }
    //}
    public static double ATan2(double y, double x)
    {
      return System.Math.Atan2(y, x);
    }

    public static float Cos(float theta)
    {
      return (float)System.Math.Cos(theta);
    }

    public static double Cos(double theta)
    {
      return System.Math.Cos(theta);
    }

    //public static float Sin(float theta) {
    //    return (float)System.Math.Sin((double)theta);

    //    //var sign = 1;

    //    //while(theta < 0) {
    //    //    theta += deg360InRadians;
    //    //}
    //    //while(theta > deg360InRadians) {
    //    //    theta -= deg360InRadians;
    //    //}

    //    //if(theta >= deg270InRadians) {
    //    //    theta -= deg270InRadians;
    //    //    sign = -1;
    //    //} else if(theta >= deg180InRadians) {
    //    //    theta -= deg180InRadians;
    //    //    sign = -1;
    //    //} else if(theta >= deg90InRadians) {
    //    //    theta -= deg90InRadians;
    //    //}

    //    //if(theta >= deg45InRadians) {
    //    //    return sign * Cos(deg90InRadians - theta);
    //    //} else {
    //    //    return sign * (theta - Pow(theta, 3) / 6 + Pow(theta, 5) / 120);
    //    //}
    //}

    public static float Sin(float theta)
    {
      return (float)System.Math.Sin(theta);
    }

    public static double Sin(double theta)
    {
      return System.Math.Sin(theta);
    }

    //public static float Tan(float theta) {
    //    return (float)System.Math.Tan((double)theta);
    //}

    public static float Tan(float theta)
    {
      return (float)System.Math.Tan(theta);
    }

    public static double Tan(double theta)
    {
      return System.Math.Tan(theta);
    }

    //public static float ATan(float theta) {
    //    var sign = 1f;
    //    if(theta < 0) {
    //        sign = -1;
    //        theta *= -1;
    //    }

    //    if(theta >= 1) {
    //        sign *= -PIm / 2;
    //        theta = 1 / theta;
    //    }

    #endregion Trig
  }
}