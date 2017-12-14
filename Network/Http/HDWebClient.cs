using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

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
        catch (Exception)
        {
          //Log.info("Web client error", e);
        }
      }

      return null;
    }

    public static bool GetHTML(
      string url,
      out string results,
      params (string key, string value)[] headers)
    {
      if (url != null)
      {
        try
        {
          var request = (HttpWebRequest)WebRequest.Create(url);
          request.AllowAutoRedirect = false;
          request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/62.0.3202.94 Safari/537.36";
          request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8";
//          GET / auctions ? type = sale & status = open & limit = 10 & sorting = cheap & orderBy = current_price & orderDirection = asc HTTP / 1.1
//Host: api.cryptokitties.co
//Connection: keep - alive
//Cache - Control: max - age = 0
//User - Agent: Mozilla / 5.0(Windows NT 10.0; Win64; x64) AppleWebKit / 537.36(KHTML, like Gecko) Chrome / 62.0.3202.94 Safari / 537.36
//Upgrade - Insecure - Requests: 1
//Accept: text / html,application / xhtml + xml,application / xml; q = 0.9,image / webp,image / apng,*/*;q=0.8
//Accept-Encoding: gzip, deflate, br
//Accept-Language: en-US,en;q=0.9
//Cookie: _ga=GA1.2.276550903.1512537969; _gid=GA1.2.586418661.1512537969
//If-None-Match: W/"201b-vTvUfviND+csJkMp8AvGfve+ODg"
          if (headers != null)
          {
            for (int i = 0; i < headers.Length; i++)
            {
              request.Headers.Add(headers[i].key, headers[i].value);
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
          Debug.Fail(e.ToString());
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
        catch (Exception)
        {
          //Log.info("Web client error", e);
        }
      }

      return null;
    }

    public static string MultipartPost(
      string url,
      (string, string)[] keyValuePairs,
      string filename = null,
      byte[] fileBytes = null,
      string fileParamName = null,
      string fileContentType = null)
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
              var formitem = $"Content-Disposition: form-data; name=\"{item.Item1}\"\r\n\r\n{item.Item2}";
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
      catch (Exception)
      {
        //Log.info(e.ToString());
        return null;
      }
    }

    // TODO does this work?
    public static string Post(
      string url,
      string body,
      (string key, string value)[] requestHeaderList,
      (string key, string value)[] contentHeaderList) // TODO remove?
    {
      HttpClient client = new HttpClient();
      //var content = new FormUrlEncodedContent(parameters);
      HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, url);
      var content = new StringContent(body);
      content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
      //content.Headers.Allow.Add("application/json");
      //for (int i = 0; i < contentHeaderList.Length; i++)
      //{
      //  var param = contentHeaderList[i];
      //  content.Headers.Add(param.key, param.value);
      //}
      requestMessage.Content = content;
      for (int i = 0; i < requestHeaderList.Length; i++)
      {
        var param = requestHeaderList[i];
        requestMessage.Headers.Add(param.key, param.value);
      }

      var response = client.SendAsync(requestMessage).Result;

      return response.Content.ReadAsStringAsync().Result;




      //try
      //{
      //  var wr = (HttpWebRequest)WebRequest.Create(url);
      //  wr.Method = "POST";
      //  wr.KeepAlive = false;
      //  //wr.Credentials = CredentialCache.DefaultCredentials;
      //  wr.ReadWriteTimeout = timeout;
      //  wr.Timeout = timeout;
      //  using (Stream rs = wr.GetRequestStream())
      //  {
      //    rs.ReadTimeout = timeout;
      //    rs.WriteTimeout = timeout;
      //    bool first = true;
      //    foreach (var item in parameters)
      //    {
      //      string prefix = first ? string.Empty : and;
      //      string formitem = $"{prefix}{item.key}={item.value}";
      //      byte[] formitembytes = Encoding.UTF8.GetBytes(formitem);
      //      rs.Write(formitembytes, 0, formitembytes.Length);
      //      first = false;
      //    }
      //    rs.Flush();
      //  }
      //  using (var response = wr.GetResponse())
      //  {
      //    using (var stream = response.GetResponseStream())
      //    {
      //      var data = stream.Read();
      //      return data.ToStringDecode();
      //    }
      //  }
      //}
      //catch (Exception e)
      //{
      //  return null;
      //}
    }

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
        catch (Exception)
        {
          //Log.error("Stream helper failed", e);
        }
      }

      return null;
    }
  }
}