using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace HD
{
  public static class AppDomainExtensions
  {

    // This did not work :(
    //public static void AddAttributeToAllClasses<AttributeType>(this AppDomain domain)
    //  where AttributeType : Attribute, new()
    //{
    //  var assemblyList = domain.GetAssemblies();
    //  for(int iAssembly = 0; iAssembly < assemblyList.Length; iAssembly++)
    //  {
    //    var assembly = assemblyList[iAssembly];
    //    var typeList = assembly.GetTypes();
    //    for(int iType = 0; iType < typeList.Length; iType++)
    //    {
    //      var type = typeList[iType];
    //      System.ComponentModel.TypeDescriptor.AddAttributes(type, new AttributeType());
    //    }
    //  }
    //}

    public static IEnumerable<(Type type, Attribute attribute)> AllClassesWithAttribute<TObjectType, TAttributeType>(
      this AppDomain domain)
      where TAttributeType : Attribute
    {
      var assemblyList = domain.GetAssemblies();
      for(int iAssembly = 0; iAssembly < assemblyList.Length; iAssembly++)
      {
        var assembly = assemblyList[iAssembly];
        var typeList = assembly.GetTypes();
        for(int iType = 0; iType < typeList.Length; iType++)
        {
          var type = typeList[iType];
          if(type.IsAssignableFrom(typeof(TObjectType)))
          {
            continue;
          }
          var attribute = Attribute.GetCustomAttribute(type, typeof(TAttributeType));
          if(attribute == null)
          {
            continue;
          }
          yield return (type, attribute);
        }
      }
    }
  }
}
