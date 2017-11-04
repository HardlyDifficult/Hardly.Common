using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace HD
{
  public static class TypeInstatiationExtensions
  {
    public static List<BaseType> InstantiateEach<BaseType, ArgType>(
        this IEnumerable<Type> types, ArgType arg)
    {
      return InstantiateEach<BaseType>(types, new[] { typeof(ArgType) }, new object[] { arg });
    }

    public static List<BaseType> InstantiateEach<BaseType, ArgType>(this List<Type> types, ArgType arg)
    {
      return InstantiateEach<BaseType>(types, new[] { typeof(ArgType) }, new object[] { arg });
    }

    public static List<BaseType> InstantiateEach<BaseType>(this List<Type> types)
    {
      return InstantiateEach<BaseType>(types, new Type[] { }, new object[] { });
    }

    public static IEnumerable<object> InstantiateEach(
        this IEnumerable<Type> types)
    {
      if (types != null)
      {
        foreach (var type in types)
        {
          yield return type.GetConstructor(new Type[] { }).Invoke(null);
        }
      }
    }

    static List<BaseType> InstantiateEach<BaseType>(
        this IEnumerable<Type> types, Type[] argTypes, object[] args)
    {
      if (types != null)
      {
        var subclassObjects = new List<BaseType>();
        foreach (var type in types)
        {
          subclassObjects.Add((BaseType)type.GetConstructor(argTypes).Invoke(args));
        }

        return subclassObjects;
      }
      Debug.Fail("Type instatiation");
      return null;
    }

    static List<BaseType> InstantiateEach<BaseType>(List<Type> types,
        Type[] argTypes, object[] args)
    {
      if (types != null)
      {
        var subclassObjects = new List<BaseType>();
        for (var i = 0; i < types.Count; i++)
        {
          subclassObjects.Add((BaseType)types[i].GetConstructor(argTypes).Invoke(args));
        }

        return subclassObjects;
      }
      return null;
    }
  }
}