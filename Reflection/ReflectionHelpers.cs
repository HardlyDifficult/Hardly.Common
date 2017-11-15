using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace HD
{
  public static class ReflectionHelpers
  {

    public static void LoadAllAssemblies()
    {
      var files = Directory.GetFiles(Environment.CurrentDirectory, "*.dll", SearchOption.TopDirectoryOnly);
      for (var i = 0; i < files.Length; i++)
      {
        Assembly.LoadFile(files[i]);
      }
    }

    public static T CreateTheFirst<T>()
    {
      Assembly[] assemblyList = AppDomain.CurrentDomain.GetAssemblies();
      for(int iAssembly = 0; iAssembly < assemblyList.Length; iAssembly++)
      {
        Assembly assembly = assemblyList[iAssembly];
        Type[] typeList = assembly.GetTypes();
        for(int iType = 0; iType < typeList.Length; iType++)
        {
          Type type = typeList[iType];
          if(typeof(T).IsAssignableFrom(type) && type.IsAbstract == false)
          {
            ConstructorInfo constructor = type.GetConstructor(new Type[] { });
            return (T)constructor.Invoke(new object[] { });
          }
        }
      }

      return default(T);
    }

    public static List<T> CreateOneOfEach<T>()
    {
      List<T> resultList = new List<T>();
      Assembly[] assemblyList = AppDomain.CurrentDomain.GetAssemblies();
      for (int iAssembly = 0; iAssembly < assemblyList.Length; iAssembly++)
      {
        Assembly assembly = assemblyList[iAssembly];
        Type[] typeList = assembly.GetTypes();
        for (int iType = 0; iType < typeList.Length; iType++)
        {
          Type type = typeList[iType];
          if (typeof(T).IsAssignableFrom(type) && type.IsAbstract == false)
          {
            ConstructorInfo constructor = type.GetConstructor(new Type[] { });
            resultList.Add((T)constructor.Invoke(new object[] { }));
          }
        }
      }

      return resultList;
    }
  }
}
