using System;

namespace HD
{
  public static class DelegateExtensions
  {
    public static bool Contains(this Delegate actionList, Delegate action)
    {
      var invocationList = actionList?.GetInvocationList();
      if(invocationList == null)
      {
        return false;
      }

      for(int i = 0; i < invocationList.Length; i++)
      {
        var currentAction = invocationList[i];
        if(currentAction == action)
        {
          return true;
        }
      }

      return false;
    }
  }
}
