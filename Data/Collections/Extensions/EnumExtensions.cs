namespace HD
{

  using System;

  public static class EnumExtensions
  {
    public static int Count<EnumType>()
    {
      return Count(typeof(EnumType));
    }

    public static int Count(Type type)
    {
      return System.Enum.GetNames(type).Length;
    }

    public static bool TryParse<EnumType>(string value, out EnumType result)
    {
      if (value != null)
      {
        var resultObject = System.Enum.Parse(typeof(EnumType), value, true);
        result = (EnumType)resultObject;
        return true;
      }

      result = default(EnumType);
      return false;
    }
  }
}