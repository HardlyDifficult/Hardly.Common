using Common.Logging;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace HD
{
  public static class IRestClientExtensions
  {
    static readonly ILog log = LogManager.GetLogger(nameof(IRestClientExtensions));

    public static async Task<(HttpStatusCode status, string content)> AsyncDownload(
      this IRestClient restClient,
      string url,
      Method method = Method.GET,
      (string key, string value)[] headers = null)
    {
      Debug.Assert(url.StartsWith("/") == false);

      return await restClient.HttpRequestAsync($"/{url}", null, method, headers);
    }

    public static async Task<(HttpStatusCode status, TJson content)> AsyncDownload<TJson>(
      this IRestClient restClient,
      string url,
      Method method = Method.GET,
      (string key, string value)[] headers = null,
      object jsonObject = null,
      CancellationToken cancellationToken = default(CancellationToken))
      where TJson : class, new()
    {
      Debug.Assert(url.StartsWith("/") == false);

      string json;
      if (jsonObject != null)
      {
        json = JsonConvert.SerializeObject(jsonObject);
      }
      else
      {
        json = null;
      }

      (HttpStatusCode status, string content)
        = await restClient.HttpRequestAsync($"/{url}", json, method, headers, cancellationToken);
      return (status, content != null ? JsonConvert.DeserializeObject<TJson>(content) : null);
    }

    static async Task<(HttpStatusCode status, string content)> HttpRequestAsync(
      this IRestClient restClient,
      string url,
      string requestJson,
      Method method,
      (string key, string value)[] headers,
      CancellationToken cancellationToken = default(CancellationToken))
    {
      log.Trace(restClient.BaseUrl + url);
      IRestRequest request = new RestRequest(url, method);

      // if you want to use fiddler
      //restClient.Proxy = new WebProxy("localhost", 8888); // IP, Port.

      request.AddHeader("Content-Type", "application/json"); //not needed, right?
      request.AddHeader("Accept", "application/json");

      if (headers != null)
      {
        for (int i = 0; i < headers.Length; i++)
        {
          (string key, string value) = headers[i];
          request.AddHeader(key, value);
        }
      }

      if (requestJson != null)
      {
        request.RequestFormat = DataFormat.Json;
        request.AddParameter("application/json", requestJson, ParameterType.RequestBody);
      }

      try
      {
        IRestResponse response = await restClient.ExecuteTaskAsync(request, cancellationToken);
        return (response.StatusCode, response.Content);
      }
      catch (Exception e)
      {
        log.Error(e.ToString());
        return (HttpStatusCode.InternalServerError, null);
      }
    }
  }
}
