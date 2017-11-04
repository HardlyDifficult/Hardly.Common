using System;
using System.Diagnostics;

namespace HD
{
  public static class FloatArrayExtensions
  {
    public static float Sample(this float[] fArr, float t)
    {
      int count = fArr.Length;
      if(count == 0)
      {
        Debug.Fail("Unable to sample array - it has no elements");
        return 0;
      }
      if(count == 1)
        return fArr[0];
      float iFloat = t * (count - 1);
      int idLower = HDMath.Floor(iFloat);
      int idUpper = HDMath.Floor(iFloat + 1);
      if(idUpper >= count)
        return fArr[count - 1];
      if(idLower < 0)
        return fArr[0];
      return HDMath.Lerp(fArr[idLower], fArr[idUpper], iFloat - idLower);
    }
  }
}
