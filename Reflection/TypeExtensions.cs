using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;

namespace HD
{
  public static class TypeExtensions
  {
    public static Func<T> CreateConstructorDelegate<T>(this Type type)
    {
      Debug.Assert(type.GetConstructor(new Type[] { }) != null, $"{type} does not have a default constructor.");

      var temp = Expression.Lambda((Expression.New(type))).Compile();
      return (Func<T>)Delegate.CreateDelegate(typeof(Func<T>), temp.Target, temp.Method);
    }
  }
}
