using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD
{
  /// <summary>
  /// Must be placed on a private static method that does nothing.
  /// Used to ensure that the static constructor for the class is called when the application starts.
  /// 
  /// TODO change to a class level attribute
  /// </summary>
  public class CallStaticConstructorOnStartAttribute : Attribute
  {
    public static void LoadAll()
    {
      var assemblyList = AppDomain.CurrentDomain.GetAssemblies();
      for (int iAssembly = 0; iAssembly < assemblyList.Length; iAssembly++)
      {
        var assembly = assemblyList[iAssembly];
        var typeList = assembly.GetTypes();
        for (int iType = 0; iType < typeList.Length; iType++)
        {
          var type = typeList[iType];
          var methodList = type.GetMethods(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
          for (int iMethod = 0; iMethod < methodList.Length; iMethod++)
          {
            var method = methodList[iMethod];
            var attributeList = method.GetCustomAttributes(typeof(CallStaticConstructorOnStartAttribute), false);
            if (attributeList == null || attributeList.Length == 0)
            {
              continue;
            }

            method.Invoke(null, null);
          }
        }
      }
    }
  }
}
