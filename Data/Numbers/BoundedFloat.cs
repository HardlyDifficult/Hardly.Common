  using System;

namespace HD
{
  public struct BoundedFloat
 {
    readonly float maxExclusive;
    float _current;

    /// <summary>
    /// [0-max)
    /// </summary>
    public BoundedFloat(float maxExclusive, float current)
    {
      this.maxExclusive = maxExclusive;
      _current = current;
      BoundValue();
    }

    public float current
    {
      get
      {
        return _current;
      }
      set
      {
        _current = value;
        BoundValue();
      }
    }

    public static implicit operator float(BoundedFloat myValue)
    {
      return myValue.current;
    }

    public override string ToString()
    {
      return current.ToString();
    }

    void BoundValue()
    {
      while (_current < 0)
      {
        _current += maxExclusive;
      }

      _current = _current % maxExclusive;
    }
  }
}