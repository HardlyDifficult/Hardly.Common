using Newtonsoft.Json;
using RestSharp;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace HD
{
  public static class IRestClientExtensions
  {
    public static async Task<T> GetAsync<T>(
      this IRestClient restClient,
      string url,
      (string key, string value)[] headers = null)
      where T : new()
    {
      Debug.Assert(url.StartsWith("/") == false);

      return await restClient.HttpRequestAsync<T>($"/{url}", null, Method.GET, headers);
    }

    static async Task<T> HttpRequestAsync<T>(
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

      IRestResponse<T> response = await restClient.ExecuteTaskAsync<T>(request);

      //var test = JsonConvert.DeserializeObject(response.Content);

      return JsonConvert.DeserializeObject<T>(response.Content);
    }

    public static T Get<T>(
      this IRestClient restClient,
      string url,
      (string key, string value)[] headers = null)
      where T : new()
    {
      Debug.Assert(url.StartsWith("/") == false);

      return restClient.HttpRequest<T>($"/{url}", null, Method.GET, headers);
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

      //var test = JsonConvert.DeserializeObject(response.Content);

      return JsonConvert.DeserializeObject<T>(response.Content);
    }
  }
}
