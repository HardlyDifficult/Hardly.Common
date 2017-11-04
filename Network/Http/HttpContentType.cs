using System;
using System.Diagnostics;

namespace HD
{
  public class HttpContentType
  {
    public static readonly HttpContentType CssContentType = new HttpContentType("text/css", new[] { ".css" });
    public static readonly HttpContentType HtmlContentType = new HttpContentType("text/html", new[] { ".htm", ".html" });
    public static readonly HttpContentType JpegContentType = new HttpContentType("image/jpeg", new[] { ".jpeg", ".jpg" });
    public static readonly HttpContentType JsContentType = new HttpContentType("application/javascript", new[] { ".js" });
    public static readonly HttpContentType OggContentType = new HttpContentType("audio/ogg", new[] { ".ogg" });
    public static readonly HttpContentType PngContentType = new HttpContentType("image/png", new[] { ".png" });
    public static readonly HttpContentType TextContentType = new HttpContentType("text/plain", new[] { ".txt" });
    public static readonly HttpContentType UnknownContentType = new HttpContentType("application/octet-stream", new string[] { });
    static readonly HttpContentType[] AllTypes = { HtmlContentType, CssContentType, TextContentType, JsContentType, JpegContentType, PngContentType, OggContentType };

    readonly string[] fileExtensions;

    readonly string type;

    public HttpContentType(string type, string[] fileExtensions)
    {
      this.type = type;
      this.fileExtensions = fileExtensions;
    }

    public static HttpContentType FromFileName(string path)
    {
      Debug.Assert(path != null && path.Trim().Length > 0 && path.Trim().Equals(path));

      for (var iType = 0; iType < AllTypes.Length; iType++)
      {
        for (var iExtension = 0; iExtension < AllTypes[iType].fileExtensions.Length; iExtension++)
        {
          if (path.EndsWith(AllTypes[iType].fileExtensions[iExtension], StringComparison.CurrentCultureIgnoreCase))
          {
            return AllTypes[iType];
          }
        }
      }

      return UnknownContentType;
    }

    public override string ToString()
    {
      return type;
    }
  }
}