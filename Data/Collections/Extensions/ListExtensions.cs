using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace HD
{
  public static class ListExtensions
  {
    #region Write API
    public static T PopLast<T>(
      this List<T> list)
    {
      if (list.Count == 0)
      {
        return default(T);
      }

      int index = list.Count - 1;
      T result = list[index];
      list.RemoveAt(index);

      return result;
    }

    public static T PopFirst<T>(
      this List<T> list)
    {
      Debug.Assert(list != null);

      if (list.Count == 0)
      {
        return default(T);
      }

      T result = list[0];
      list.RemoveAt(0);

      return result;
    }

    public static void Swap<T>(
      this List<T> list,
      int indexA,
      int indexB)
    {
      if (indexA == indexB || list == null)
      {
        return;
      }

      T temp = list[indexA];
      list[indexA] = list[indexB];
      list[indexB] = temp;
    }

    public static void BubbleUpto<T>(
      this List<T> list,
      int indexFrom,
      int indexTo)
    {
      if (indexFrom == indexTo)
      {
        return;
      }

      for (int i = indexFrom; i > indexTo; i--)
      {
        list.Swap(i, i - 1);
      }
    }
    #endregion

    #region Read API
    public static bool ContainsType<TItem>(
      this List<TItem> list,
      Type type)
    {
      if (list == null)
      {
        return false;
      }

      for (int i = 0; i < list.Count; i++)
      {
        if (list[i].GetType() == type)
        {
          return true;
        }
      }

      return false;
    }

    public static T GetMax<T>(
      this List<T> list)
      where T : IComparable<T>
    {
      if (list.Count == 0)
      {
        return default(T);
      }

      T max = list[0];
      for (int i = 1; i < list.Count; i++)
      {
        T item = list[i];
        if (item.CompareTo(max) > 0)
        {
          max = item;
        }
      }

      return max;
    }

    public static string ToCsv<T>(
      this List<T> list)
    {
      if (list == null)
      {
        return "NULL list";
      }
      if (list.Count == 0)
      {
        return "Empty list";
      }

      StringBuilder builder = new StringBuilder();
      for (int i = 0; i < list.Count; i++)
      {
        T item = list[i];
        if (i > 0)
        {
          builder.Append(", ");
        }
        builder.Append(item);
      }

      return builder.ToString();
    }

    public static bool TryFindByType<TList>(
      this List<TList> list,
      TList item,
      out TList existingItem)
      where TList : class
    {
      if (list == null)
      {
        existingItem = null;
        return false;
      }

      for (int i = 0; i < list.Count; i++)
      {
        if (list[i].GetType() == item.GetType())
        {
          existingItem = list[i];
          return true;
        }
      }

      existingItem = null;
      return false;
    }

    public static ValueType PeekLast<ValueType>(
      this List<ValueType> list)
    {
      if (list == null)
      {
        return default(ValueType);
      }

      var index = list.Count - 1;
      if (index < 0)
      {
        return default(ValueType);
      }

      return list[index];
    }

    public static uint CountIf<T>(
      this List<T> list,
      Func<T, bool> ifCondition)
    {
      uint count = 0;
      for (int i = 0; i < list.Count; i++)
      {
        T item = list[i];
        if (ifCondition(item))
        {
          count++;
        }
      }

      return count;
    }
    #endregion
  }
}
