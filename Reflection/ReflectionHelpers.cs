using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace HD
{
  public static class ReflectionHelpers
  {
    /// <summary>
    /// Loads all .dll files in the current process's directory
    /// 
    /// TODO can we store a bit or something so this only happens once?
    /// </summary>
    public static void LoadAllAssemblies()
    {
      var files = Directory.GetFiles(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
        "*.dll", SearchOption.TopDirectoryOnly);
      for (var i = 0; i < files.Length; i++)
      {
        try
        {
          Assembly.LoadFile(files[i]);
        }
        catch { } // C++ will throw exceptions..
      }
    }

    public static T CreateTheFirst<T>()
    {
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
            FieldInfo instanceField = type.GetField("instance", BindingFlags.Static | BindingFlags.Public);
            if (instanceField != null && typeof(T).IsAssignableFrom(instanceField.FieldType))
            {
              object tObject = instanceField.GetValue(null);
              Debug.Assert(tObject != null);
              resultList.Add((T)tObject);
            }
            else
            {
              ConstructorInfo constructor = type.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public, null, new Type[] { }, null);
              Debug.Assert(constructor != null);

              resultList.Add((T)constructor.Invoke(new object[] { }));
            }
          }
        }
      }

      return resultList;
    }
  }
}
