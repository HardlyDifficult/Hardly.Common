using System;
using System.Collections.Generic;
using System.Linq;

namespace HD
{
  public static class ArrayExtensions
  {
    #region Write
    public static T[] Append<T>(
      this T[] a, 
      T[] b)
    {
      if (a != null && b != null)
      {
        try
        {
          var length = a.Length + b.Length;
          var merged = new T[length];
          var i = 0;
          for (var iA = 0; iA < a.Length; iA++)
          {
            merged[i++] = a[iA];
          }
          for (var iB = 0; iB < b.Length; iB++)
          {
            merged[i++] = b[iB];
          }

          return merged;
        }
        catch (Exception e)
        {
          //Log.error("Array helper merge failed", e);
        }
      }
      else if (a != null)
      {
        return a;
      }
      else if (b != null)
      {
        return b;
      }

      //Debug.Fail("Array append");
      return null;
    }

    public static T[] Append<T>(
      this T[] a, 
      T b)
    {
      if (a != null && b != null)
      {
        try
        {
          var length = a.Length + 1;
          var merged = new T[length];
          var i = 0;
          for (var iA = 0; iA < a.Length; iA++)
          {
            merged[i++] = a[iA];
          }
          merged[i++] = b;

          return merged;
        }
        catch (Exception e)
        {
          //Log.error("Array helper merge failed", e);
        }
      }
      else if (a != null)
      {
        return a;
      }
      else if (b != null)
      {
        return new[] { b };
      }

      return null;
    }

    public static void Clear<T>(
      this T[] data)
    {
      if (data != null)
      {
        Array.Clear(data, 0, data.Length);
      }
    }

    public static void Clear<T>(
      this T[,] data)
    {
      var defaultValue = default(T);
      for (var i = 0; i < data.GetLength(0); i++)
      {
        for (var j = 0; j < data.GetLength(1); j++)
        {
          data[i, j] = defaultValue;
        }
      }
    }

    public static T[] DuplicateEntities<T>(
      this T[] data, uint numberOfTimes)
    {
      var finalData = data;
      if (numberOfTimes > 0)
      {
        for (var i = 0; i < numberOfTimes; i++)
        {
          finalData = finalData.Append(data);
        }
      }

      return finalData;
    }

    public static void Reverse<T>(this T[] list)
    {
      for (int i = 0; i < list.Length / 2; i++)
      {
        var temp = list[i];
        var iOther = list.Length - i - 1;
        list[i] = list[iOther];
        list[iOther] = temp;
      }
    }

    public static void Remove<T>(
      this T[] list, 
      T item) where T : class
    {
      for (var i = 0; i < list.Length; i++)
      {
        if (list[i] == item)
        {
          list[i] = null;
          return;
        }
      }
    }
    #endregion

    #region Read
    public static bool Contains<T>(
      this T[] data, 
      T item)
        where T : IEqualityComparer<T>
    {
      for (var i = 0; i < data.Length; i++)
      {
        if (data[i] == null)
        {
          if (item == null)
          {
            return true;
          }
        }
        else if (data[i].Equals(item))
        {
          return true;
        }
      }

      return false;
    }

    public static bool Contains<T>(
      this List<T>[] data, 
      T item)
    {
      for (var i = 0; i < data.Length; i++)
      {
        if (data[i] == null)
        {
          if (item == null)
          {
            return true;
          }
        }
        else if (data[i].Contains(item))
        {
          return true;
        }
      }

      return false;
    }

    public static bool Contains<T>(
      this List<T>[,] data, 
      T item)
    {
      for (var i = 0; i < data.GetLength(0); i++)
      {
        for (var j = 0; j < data.GetLength(1); j++)
        {
          if (data[i, j] == null)
          {
            if (item == null)
            {
              return true;
            }
          }
          else if (data[i, j].Contains(item))
          {
            return true;
          }
        }
      }

      return false;
    }

    public static int CountEntries<T>(
      this T[][] list)
    {
      int count = 0;
      for (int i = 0; i < list.Length; i++)
      {
        count += list[i].Length;
      }

      return count;
    }

    public static T GetFirstInstanceOf<T>(
      this object[] items)
    {
      if (items != null)
      {
        for (var i = 0; i < items.Length; i++)
        {
          if (items[i] is T)
          {
            return (T)items[i];
          }
        }
      }

      return default(T);
    }

    public static bool HasAnObject<T>(
      this T[] list) where T : class
    {
      if (list != null)
      {
        for (var i = 0; i < list.Length; i++)
        {
          if (list[i] != null)
          {
            return true;
          }
        }
      }

      return false;
    }

    public static string GetLongestString(
      this string[] list)
    {
      var longest = (string)null;
      if (list != null)
      {
        for (int i = 0; i < list.Length; i++)
        {
          if (longest == null || list[i].Length > longest.Length)
          {
            longest = list[i];
          }
        }
      }
      return longest;
    }

    public static bool HasContent<T>(
      this List<T>[] list)
    {
      if (list != null)
      {
        for (var i = 0; i < list.Length; i++)
        {
          if (list[i].Count > 0)
          {
            return true;
          }
        }
      }

      return false;
    }

    public static bool HasNull<T>(
      this T[] list)
      where T : class
    {
      for (int i = 0; i < list.Length; i++)
      {
        if (list[i] == null)
        {
          return true;
        }
      }

      return false;
    }

    public static int IndexOf<T>(
      this T[] array,
      T value)
    {
      return Array.IndexOf(array, value);
    }

    public static bool IsCompletelyNull<T>(
      this T[] list)
      where T : class
    {
      for (int i = 0; i < list.Length; i++)
      {
        var item = list[i];
        if (item != null)
        {
          return false;
        }
      }

      return true;
    }

    public static T[] CloneAndShuffle<T>(
      this T[] list)
    {
      return list.OrderBy(a => Guid.NewGuid()).ToArray();
    }

    public static T[] SubArray<T>(
      this T[] data, 
      uint index, 
      uint length)
    {
      if (data != null && data.Length >= 0)
      {
        try
        {
          if (length == 0 || index + length > data.Length)
          {
            length = (uint)data.Length - index;
          }

          var result = new T[length];
          Array.Copy(data, index, result, 0, length);
          return result;
        }
        catch (Exception e)
        {
          //Log.error("Sub-array helper failed", e);
        }
      }
      else
      {
        //Debug.Fail("Array sub array");
      }

      return null;
    }

    public static T[] ToArray<T>(
      this IEnumerable<T> list)
    {
      if (list != null)
      {
        var results = Enumerable.ToArray(list);
        if (results.Length > 0)
        {
          return results;
        }
      }

      return null;
    }
    #endregion
  }
}