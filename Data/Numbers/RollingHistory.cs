using System;

namespace HD
{
  public class RollingHistory
  {
    double[] historyList;
    int index;

    public bool hasFilledOnce
    {
      get; private set;
    }

    public double value
    {
      get; private set;
    }

    public RollingHistory(
      int numberOfSamples)
    {
      historyList = new double[numberOfSamples];
    }

    public void Add(
      double value)
    {
      historyList[index++] = value;
      if (index == historyList.Length)
      {
        index = 0;
        hasFilledOnce = true;
      }
      double newValue = 0;
      double max, min;
      max = min = historyList[0];
      int count = hasFilledOnce ? historyList.Length : index;
      for (int i = 0; i < count; i++)
      {
        double v = historyList[i];
        newValue += v;
        if (v > max)
        {
          max = v;
        }
        else if (v < min)
        {
          min = v;
        }
      }
      if (count > 2)
      {
        newValue -= max + min;
        newValue /= count - 2;
      }
      this.value = newValue;
    }

    public void Clear()
    {
      index = 0;
      value = 0;
      hasFilledOnce = false;
    }
  }
}
