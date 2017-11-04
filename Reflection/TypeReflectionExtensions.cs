using System;
using System.Collections.Generic;
using System.Linq;

namespace HD
{
  public static class TypeReflectionExtensions
  {
    public enum ClassType
    {
      Concrete, Abstract, Interface
    }

    public static IEnumerable<Type> GetAllSubclasses(this Type parentType,
        string includeJustThisNamespace = null, bool includeAllAssemblies = true,
        ClassType type = ClassType.Concrete)
    {
      if (parentType != null)
      {
        var assemblies = (includeAllAssemblies ? AppDomain.CurrentDomain.GetAssemblies() : new[] { parentType.Assembly });
        for (var i = 0; i < assemblies.Length; i++)
        {
          foreach (var thing in assemblies[i].GetTypes().Where(t => parentType.IsAssignableFrom(t)
                 && (includeJustThisNamespace == null || includeJustThisNamespace.Equals(t.Namespace))
                ))
          {
            if (type == ClassType.Concrete && thing.IsAbstract)
            {
              continue;
            }

            if (type == ClassType.Abstract && !thing.IsAbstract)
            {
              continue;
            }

            if (type == ClassType.Interface && !thing.IsInterface)
            {
              continue;
            }

            yield return thing;
          }
        }
      }
    }
  }
}