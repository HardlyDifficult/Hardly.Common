using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace HD
{
  public static class HDWebClient
  {
    const string and = "&";
    const int timeout = 7000;

    static HDWebClient()
    {
      ServicePointManager.ServerCertificateValidationCallback = YesAlwaysYes;
    }

    public static byte[] GetBytes(string url)
    {
      if (url != null && url.Trim().Length > 0)
      {
        try
        {
          var request = (HttpWebRequest)WebRequest.Create(url);
          request.AllowAutoRedirect = false;
          request.Timeout = timeout;
          using (var response = request.GetResponse())
          {
            var input = response.GetResponseStream();
            var buffer = new byte[16 * 1024];
            using (var ms = new MemoryStream())
            {
              var read = input.Read(buffer, 0, buffer.Length);
              while (read > 0)
              {
                ms.Write(buffer, 0, read);
                read = input.Read(buffer, 0, buffer.Length);
              }
              return ms.ToArray();
            }
          }
        }
        catch (Exception e)
        {
          //Log.info("Web client error", e);
        }
      }

      return null;
    }

    public static bool GetHTML(string url, out string results, Tuple<string, string>[] headers = null)
    {
      if (url != null)
      {
        try
        {
          var request = (HttpWebRequest)WebRequest.Create(url);
          request.AllowAutoRedirect = false;
          if (headers != null)
          {
            for (var i = 0; i < headers.Length; i++)
            {
              request.Headers.Add(headers[i].Item1, headers[i].Item2);
            }
          }
          request.Timeout = timeout;
          request.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);
          using (var response = request.GetResponse())
          {
            using (var reader = new StreamReader(response.GetResponseStream()))
            {
              results = reader.ReadToEnd();
              if (results != null)
              {
                return true;
              }
            }
          }
        }
        catch (WebException e)
        {
          using (var response = e.Response)
          {
            if (response != null)
            {
              using (var reader = new StreamReader(response.GetResponseStream()))
              {
                results = reader.ReadToEnd();
                if (!string.IsNullOrEmpty(results))
                {
                  return false;
                }
              }
            }
          }
        }
        catch (Exception e)
        {
          //Log.info("Web client error", e);
        }
      }

      results = null;
      return false;
    }

    public static WebResponse GetStream(string url)
    {
      if (url != null)
      {
        try
        {
          var request = (HttpWebRequest)WebRequest.Create(url);
          request.AllowAutoRedirect = false;
          request.Timeout = timeout;
          request.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);
          return request.GetResponse();
        }
        catch (Exception e)
        {
          //Log.info("Web client error", e);
        }
      }

      return null;
    }

    public static string Post(string url, Dictionary<string, string> keyValuePairs,
        string filename = null, byte[] fileBytes = null,
        string fileParamName = null, string fileContentType = null)
    {
      try
      {
        var boundary = $"---------------------------{DateTime.Now.Ticks:x}";
        var boundarybytes = Encoding.ASCII.GetBytes($"--{boundary}\r\n");
        var newLine = Encoding.ASCII.GetBytes("\r\n");

        var wr = (HttpWebRequest)WebRequest.Create(url);
        wr.AllowAutoRedirect = false;
        wr.ContentType = $"multipart/form-data; boundary={boundary}";
        wr.Method = "POST";
        wr.KeepAlive = false;
        wr.Credentials = CredentialCache.DefaultCredentials;
        wr.ReadWriteTimeout = timeout;
        wr.Timeout = timeout;
        using (var rs = wr.GetRequestStream())
        {
          rs.ReadTimeout = timeout;
          rs.WriteTimeout = timeout;
          if (keyValuePairs != null)
          {
            foreach (var item in keyValuePairs)
            {
              rs.Write(boundarybytes, 0, boundarybytes.Length);
              var formitem = $"Content-Disposition: form-data; name=\"{item.Key}\"\r\n\r\n{item.Value}";
              var formitembytes = Encoding.UTF8.GetBytes(formitem);
              rs.Write(formitembytes, 0, formitembytes.Length);
              rs.Write(newLine, 0, newLine.Length);
            }
          }
          if (filename != null)
          {
            rs.Write(boundarybytes, 0, boundarybytes.Length);
            var header = $"Content-Disposition: form-data; name=\"{fileParamName}\"; filename=\"{filename}\"\r\nContent-Type: {fileContentType}\r\n\r\n";
            var headerbytes = Encoding.UTF8.GetBytes(header);
            rs.Write(headerbytes, 0, headerbytes.Length);
            if (fileBytes != null)
            {
              rs.Write(fileBytes, 0, fileBytes.Length);
            }
          }

          var trailer = Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
          rs.Write(trailer, 0, trailer.Length);
          rs.Flush();
        }
        using (var response = wr.GetResponse())
        {
          using (var stream = response.GetResponseStream())
          {
            var data = stream.Read();
            return data.ToStringDecode();
          }
        }
      }
      catch (Exception e)
      {
        //Log.info(e.ToString());
        return null;
      }
    }

    // does not appear to be a valid post
    //public static string Post(string url, Dictionary<string, string> keyValuePairs) {
    //    try {
    //        Log.info($"Requesting url: {url}");
    //        var wr = (HttpWebRequest)WebRequest.Create(url);
    //        wr.Method = "POST";
    //        wr.KeepAlive = false;
    //        wr.Credentials = CredentialCache.DefaultCredentials;
    //        wr.ReadWriteTimeout = timeout;
    //        wr.Timeout = timeout;
    //        using(var rs = wr.GetRequestStream()) {
    //            rs.ReadTimeout = timeout;
    //            rs.WriteTimeout = timeout;
    //            var first = true;
    //            foreach(var item in keyValuePairs) {
    //                var prefix = first ? string.Empty : and;
    //                var formitem = $"{prefix}{item.Key}={item.Value}";
    //                var formitembytes = Encoding.UTF8.GetBytes(formitem);
    //                rs.Write(formitembytes, 0, formitembytes.Length);
    //                first = false;
    //            }
    //            rs.Flush();
    //        }
    //        using(var response = wr.GetResponse()) {
    //            using(var stream = response.GetResponseStream()) {
    //                var data = stream.Read();
    //                return data.ToStringDecode();
    //            }
    //        }
    //    } catch(Exception e) {
    //        Log.error("WebClient POST", e);
    //        return null;
    //    }
    //}

    static bool YesAlwaysYes(object sender, X509Certificate certificate,
        X509Chain chain, SslPolicyErrors sslPolicyErrors)
    {
      return true;
    }
  }

  public static class StreamHelpers
  {
    public static byte[] Read(this Stream stream)
    {
      if (stream != null && stream.CanRead)
      {
        try
        {
          var buffer = new byte[32768];
          using (var ms = new MemoryStream())
          {
            while (true)
            {
              var read = stream.Read(buffer, 0, buffer.Length);
              if (read <= 0)
              {
                return ms.ToArray();
              }
              ms.Write(buffer, 0, read);
            }
          }
        }
        catch (Exception e)
        {
          //Log.error("Stream helper failed", e);
        }
      }

      return null;
    }
  }
}