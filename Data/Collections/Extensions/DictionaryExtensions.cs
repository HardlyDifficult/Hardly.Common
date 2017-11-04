using System;
using System.Collections.Generic;

namespace HD
{
  public static class DictionaryExtensions
  {
    #region 1 key
    public static List<TKey> ToList<TKey, TValue>(
      this Dictionary<TKey, TValue>.KeyCollection dictionaryKeys)
    {
      List<TKey> keyList = new List<TKey>();
      foreach(var item in dictionaryKeys)
      {
        keyList.Add(item);
      }

      return keyList;
    }

    public static void Add<TKey, TValue>(
      this Dictionary<TKey, List<TValue>> dictionary,
        TKey key,
        TValue value)
    {
      if(dictionary.TryGetValue(key, out List<TValue> list) == false)
      {
        list = new List<TValue>();
        dictionary.Add(key, list);
      }

      list.Add(value);
    }

    public static void Remove<TKey, TValue>(
      this Dictionary<TKey, List<TValue>> dictionary,
      TKey key,
      TValue value)
    {
      if(dictionary.TryGetValue(key, out List<TValue> list) == false)
      {
        return;
      }

      list.Remove(value);
      if(list.Count == 0)
      {
        dictionary.Remove(key);
      }
    }

    public static bool TryPop<TKey, TValue>(
      this Dictionary<TKey, List<TValue>> dictionary,
      TKey key,
      out TValue value)
    {
      if(dictionary.TryGetValue(key, out List<TValue> list) == false)
      {
        value = default(TValue);
        return false;
      }

      value = list.PopLast();
      if(list.Count == 0)
      {
        dictionary.Remove(key);
      }
      return true;
    }
    #endregion

    #region 2 keys
    public static void Add<PrimaryKeyType, SecondaryKeyType, ValueType>(
      this Dictionary<PrimaryKeyType, Dictionary<SecondaryKeyType, ValueType>> dictionary,
      PrimaryKeyType primaryKey,
      SecondaryKeyType secondaryKey,
      ValueType value)
    {
      if(dictionary.TryGetValue(primaryKey, out Dictionary<SecondaryKeyType, ValueType> innerDictionary))
      {
        innerDictionary.Add(secondaryKey, value);
      }
      else
      { // primary key is new
        innerDictionary = new Dictionary<SecondaryKeyType, ValueType>
        {
          { secondaryKey, value }
        };
        dictionary.Add(primaryKey, innerDictionary);
      }
    }

    public static void Remove<PrimaryKeyType, SecondaryKeyType, ValueType>(
     this Dictionary<PrimaryKeyType, Dictionary<SecondaryKeyType, ValueType>> dictionary,
     PrimaryKeyType primaryKey,
     SecondaryKeyType secondaryKey)
    {
      Dictionary<SecondaryKeyType, ValueType> innerDictionary = dictionary[primaryKey];

      innerDictionary.Remove(secondaryKey);

      if(innerDictionary.Count == 0)
      {
        dictionary.Remove(primaryKey);
      }
    }

    public static bool TryGetValue<PrimaryKeyType, SecondaryKeyType, ValueType>(
      this Dictionary<PrimaryKeyType, Dictionary<SecondaryKeyType, ValueType>> dictionary,
      PrimaryKeyType primaryKey,
      SecondaryKeyType secondaryKey,
      out ValueType value)
    {
      if(dictionary.TryGetValue(primaryKey, out Dictionary<SecondaryKeyType, ValueType> innerDictionary))
      {
        if(innerDictionary.TryGetValue(secondaryKey, out value))
        {
          return true;
        }
      }

      value = default(ValueType);
      return false;
    }
    #endregion

    #region 3 keys
    public static void Add<TKey1, TKey2, TKey3, TValue>(
      this Dictionary<TKey1, Dictionary<TKey2, Dictionary<TKey3, TValue>>> dictionary,
      TKey1 key1,
      TKey2 key2,
      TKey3 key3,
      TValue value)
    {
      if(dictionary.TryGetValue(key1, out Dictionary<TKey2, Dictionary<TKey3, TValue>> level2Dict))
      {
        if(level2Dict.TryGetValue(key2, out Dictionary<TKey3, TValue> level3Dict))
        {
          level3Dict.Add(key3, value);
        }
        else
        {
          level3Dict = new Dictionary<TKey3, TValue>
          {
            { key3, value }
          };
          level2Dict.Add(key2, level3Dict);
        }
      }
      else
      { // primary key is new
        var level3Dict = new Dictionary<TKey3, TValue>()
        {
          {key3, value }
        };
        level2Dict = new Dictionary<TKey2, Dictionary<TKey3, TValue>>()
        {
          {key2, level3Dict }
        };
        dictionary.Add(key1, level2Dict);
      }
    }

    public static void Remove<TKey1, TKey2, TKey3, TValue>(
     this Dictionary<TKey1, Dictionary<TKey2, Dictionary<TKey3, TValue>>> dictionary,
     TKey1 key1,
     TKey2 key2,
     TKey3 key3)
    {
      Dictionary<TKey2, Dictionary<TKey3, TValue>> level2Dict = dictionary[key1];
      Dictionary<TKey3, TValue> level3Dict = level2Dict[key2];
      level3Dict.Remove(key3);

      if(level3Dict.Count == 0)
      {
        level2Dict.Remove(key2);

        if(level2Dict.Count == 0)
        {
          dictionary.Remove(key1);
        }
      }
    }

    public static bool TryGetValue<TKey1, TKey2, TKey3, TValue>(
      this Dictionary<TKey1, Dictionary<TKey2, Dictionary<TKey3, TValue>>> dictionary,
      TKey1 key1,
      TKey2 key2,
      TKey3 key3,
      out TValue value)
    {
      if(dictionary.TryGetValue(key1, out Dictionary<TKey2, Dictionary<TKey3, TValue>> innerDictionary))
      {
        if(innerDictionary.TryGetValue(key2, out Dictionary<TKey3, TValue> level3Dict))
        {
          if(level3Dict.TryGetValue(key3, out value))
          {
            return true;
          }
        }
      }

      value = default(TValue);
      return false;
    }
    #endregion

    #region 4 keys
    public static void Add<TKey1, TKey2, TKey3, TKey4, TValue>(
      this Dictionary<TKey1, Dictionary<TKey2, Dictionary<TKey3, Dictionary<TKey4, TValue>>>> dictionary,
      TKey1 key1,
      TKey2 key2,
      TKey3 key3,
      TKey4 key4,
      TValue value)
    {
      if(dictionary.TryGetValue(key1, out Dictionary<TKey2, Dictionary<TKey3, Dictionary<TKey4, TValue>>> level2Dict))
      {
        if(level2Dict.TryGetValue(key2, out Dictionary<TKey3, Dictionary<TKey4, TValue>> level3Dict))
        {
          if(level3Dict.TryGetValue(key3, out Dictionary<TKey4, TValue> level4Dict))
          { // Add value
            level4Dict.Add(key4, value);
          }
          else
          { // Add key 4 and value
            level4Dict = new Dictionary<TKey4, TValue>();
            level4Dict.Add(key4, value);
            level3Dict.Add(key3, level4Dict);
          }
        }
        else
        { // Add key 3, 4, and value
          Dictionary<TKey4, TValue> level4Dict = new Dictionary<TKey4, TValue>();
          level4Dict.Add(key4, value);

          level3Dict = new Dictionary<TKey3, Dictionary<TKey4, TValue>>();
          level3Dict.Add(key3, level4Dict);

          level2Dict.Add(key2, level3Dict);
        }
      }
      else
      { // primary key is new
        Dictionary<TKey4, TValue> level4Dict = new Dictionary<TKey4, TValue>();
        level4Dict.Add(key4, value);

        Dictionary<TKey3, Dictionary<TKey4, TValue>> level3Dict = new Dictionary<TKey3, Dictionary<TKey4, TValue>>();
        level3Dict.Add(key3, level4Dict);

        level2Dict = new Dictionary<TKey2, Dictionary<TKey3, Dictionary<TKey4, TValue>>>();
        level2Dict.Add(key2, level3Dict);

        dictionary.Add(key1, level2Dict);
      }
    }

    public static void Remove<TKey1, TKey2, TKey3, TKey4, TValue>(
     this Dictionary<TKey1, Dictionary<TKey2, Dictionary<TKey3, Dictionary<TKey4, TValue>>>> dictionary,
     TKey1 key1,
     TKey2 key2,
     TKey3 key3,
     TKey4 key4)
    {
      Dictionary<TKey2, Dictionary<TKey3, Dictionary<TKey4, TValue>>> level2Dict = dictionary[key1];
      Dictionary<TKey3, Dictionary<TKey4, TValue>> level3Dict = level2Dict[key2];
      Dictionary<TKey4, TValue> level4Dict = level3Dict[key3];
      level4Dict.Remove(key4);

      if(level4Dict.Count == 0)
      {
        level3Dict.Remove(key3);

        if(level3Dict.Count == 0)
        {
          level2Dict.Remove(key2);

          if(level2Dict.Count == 0)
          {
            dictionary.Remove(key1);
          }
        }
      }
    }

    public static bool TryGetValue<TKey1, TKey2, TKey3, TKey4, TValue>(
      this Dictionary<TKey1, Dictionary<TKey2, Dictionary<TKey3, Dictionary<TKey4, TValue>>>> dictionary,
      TKey1 key1,
      TKey2 key2,
      TKey3 key3,
      TKey4 key4,
      out TValue value)
    {
      if(dictionary.TryGetValue(key1, out Dictionary<TKey2, Dictionary<TKey3, Dictionary<TKey4, TValue>>> level2Dict))
      {
        if(level2Dict.TryGetValue(key2, out Dictionary<TKey3, Dictionary<TKey4, TValue>> level3Dict))
        {
          if(level3Dict.TryGetValue(key3, out Dictionary<TKey4, TValue> level4Dict))
          {
            if(level4Dict.TryGetValue(key4, out value))
            {
              return true;
            }
          }
        }
      }

      value = default(TValue);
      return false;
    }
    #endregion

    public static void Add<KeyType, Value>(this Dictionary<KeyType,
        LinkedList<Value>> dictionary, KeyType key, Value value)
        where KeyType : IEquatable<KeyType>
        where Value : class
    {
      LinkedList<Value> list;
      if (dictionary.TryGetValue(key, out list))
      {
        list.AddLast(value);
      }
      else
      {
        list = new LinkedList<Value>();
        list.AddLast(value);
        dictionary.Add(key, list);
      }
    }

    public static bool Remove<KeyType, Value>(this Dictionary<KeyType, LinkedList<Value>> dictionary,
        KeyType key, Value value)
        where KeyType : IEquatable<KeyType>
        where Value : class
    {
      LinkedList<Value> list;
      if (dictionary.TryGetValue(key, out list))
      {
        if (list.Count > 1)
        {
          return list.Remove(value);
        }
        if (list.First.Value.Equals(value))
        {
          dictionary.Remove(key);
        }
      }

      return false;
    }

    public static bool Remove<KeyType, ListItemType>(this Dictionary<KeyType, List<ListItemType>> dictionary, ListItemType item)
        where KeyType : IEquatable<KeyType>
    {
      foreach (var value in dictionary.Values)
      {
        if (value.Remove(item))
        {
          return true;
        }
      }

      return false;
    }

    public static int ValueCount<KeyType, Value>(this Dictionary<KeyType, List<Value>> dictionary)
                                        where KeyType : IEquatable<KeyType>
    {
      int count = 0;
      foreach (var value in dictionary.Values)
      {
        if (value != null)
        {
          count += value.Count;
        }
      }

      return count;
    }
  }
}
