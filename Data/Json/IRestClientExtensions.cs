using Common.Logging;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace HD
{
  public static class IRestClientExtensions
  {
    static readonly ILog log = LogManager.GetLogger(nameof(IRestClientExtensions));

    public static async Task<string> AsyncDownload(
      this IRestClient restClient,
      string url,
      Method method = Method.GET,
      (string key, string value)[] headers = null)
    {
      Debug.Assert(url.StartsWith("/") == false);

      return await restClient.HttpRequestAsync($"/{url}", null, method, headers);
    }

    static async Task<string> HttpRequestAsync(
      this IRestClient restClient,
      string url,
      string requestJson,
      Method method,
      (string key, string value)[] headers)
    {
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

      IRestResponse response = await restClient.ExecuteTaskAsync(request);
      return response.Content;
    }

    public static async Task<T> AsyncDownload<T>(
      this IRestClient restClient,
      string url,
      Method method = Method.GET,
      (string key, string value)[] headers = null,
      object jsonObject = null,
      CancellationToken cancellationToken = default(CancellationToken))
      where T : new()
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

      return await restClient.HttpRequestAsync<T>($"/{url}", json, method, headers, cancellationToken);
    }

    static async Task<T> HttpRequestAsync<T>(
      this IRestClient restClient,
      string url,
      string requestJson,
      Method method,
      (string key, string value)[] headers,
      CancellationToken cancellationToken)
      where T : new()
    {
      log.Trace(url);

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
        IRestResponse<T> response = await restClient.ExecuteTaskAsync<T>(request, cancellationToken);
        return JsonConvert.DeserializeObject<T>(response.Content);
      }
      catch (TaskCanceledException e)
      {
        log.Error("Task canceled", e);
      }
      catch (Exception e)
      { // Parsing error
        log.Error("Json parsing error", e);
      }

      return default(T);
    }

    public static T Download<T>(
      this IRestClient restClient,
      string url,
      Method method = Method.GET,
      (string key, string value)[] headers = null)
      where T : new()
    {
      Debug.Assert(url.StartsWith("/") == false);

      return restClient.HttpRequest<T>($"/{url}", null, method, headers);
    }

    static T HttpRequest<T>(
      this IRestClient restClient,
      string url,
      string requestJson,
      Method method,
      (string key, string value)[] headers)
      where T : new()
    {
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

      IRestResponse<T> response = restClient.Execute<T>(request);

      try
      {
        return JsonConvert.DeserializeObject<T>(response.Content);
      }
      catch (Exception e)
      { // Parsing error
        log.Error("Json parsing error", e);
        return default(T);
      }
    }
  }
}
