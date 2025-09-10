using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TrackMyTrain.Data.Interfaces;
using TrackMyTrain.Maui.Utilities;

namespace TrackMyTrain.Maui.Services
{
    public class ApiClientService : IApiClientService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<ApiClientService> _logger;
        public ApiClientService(IHttpClientFactory httpClientFactory, ILogger<ApiClientService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<TOut> GetAsync<TOut, TIn>(string url, TIn q, bool authRequired = true, bool isPlainText = false, bool retry = true, CancellationToken? cancellationToken = null)
            where TOut : class
            where TIn : class
        {
            var result = await _GetAsync<TOut, TIn>(url, q, authRequired, isPlainText, retry, cancellationToken);
            return result;
        }

        public async Task<TOut> GetAsync<TOut>(string url, bool authRequired = true, bool isPlainText = false, bool retry = true, CancellationToken? cancellationToken = null) where TOut : class
        {
            var result = await _GetAsync<TOut, Void>(url, Void.Empty, authRequired, isPlainText, retry, cancellationToken);
            return result;
        }

        public async Task GetAsync<TIn>(string url, TIn q, bool authRequired = true, bool isPlainText = false, bool retry = true, CancellationToken? cancellationToken = null) where TIn : class
        {
            await _GetAsync<Void, TIn>(url, q, authRequired, isPlainText, retry, cancellationToken);
        }

        public async Task<TOut> PostAsync<TOut, TIn>(string url, TIn body, bool authRequired = true, bool retry = true)
            where TOut : class
            where TIn : class
        {
            var result = await _PostAsync<TOut, TIn>(url, body, authRequired, retry);
            return result;
        }

        public Task<TOut> PostAsync<TOut>(string url, bool authRequired = true, bool retry = true) where TOut : class
        {
            var result = _PostAsync<TOut, Void>(url, Void.Empty, authRequired, retry);
            return result;
        }

        public async Task PostAsync<TIn>(string url, TIn body, bool authRequired = true, bool retry = true) where TIn : class
        {
            await _PostAsync<Void, TIn>(url, body, authRequired, retry);
        }

        private async Task<TOut> _PostAsync<TOut, TIn>(string url, TIn body, bool authRequired = true, bool retry = true) where TOut : class where TIn : class
        {
            string json = string.Empty;
            try
            {
                // TODO MARCO
                var checkInternet = GenericUtilities.CheckInternetConnection();
                if (!checkInternet)
                {
                    throw new Exception("No internet connection");
                }
                using (var client = _httpClientFactory.CreateClient("api"))
                {
                    if (authRequired)
                    {
                        // TODO MARCO
                        //var bearer = await SecureStorage.GetAsync("access_token");
                        //client.DefaultRequestHeaders.Add($"Authorization", $"Bearer {bearer}");
                    }

                    json = JsonSerializer.Serialize(body);
                    HttpContent content = null;
                    if (typeof(TIn) != typeof(Void))
                    {
                        content = new StringContent(json, Encoding.UTF8, "application/json");
                    }
                    var result = client.PostAsync(url, content).Result;
                    if (result.IsSuccessStatusCode)
                    {
                        if (typeof(TOut) == typeof(Void))
                            return Void.Empty as TOut;
                        var response = await result.Content.ReadAsStringAsync();
                        _logger.LogInformation(" === API === " + url + " => " + (string.IsNullOrEmpty(json) ? "" : json) + " " + response);
                        var returnValue = JsonSerializer.Deserialize<TOut>(response, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });
                        return returnValue;
                    }
                    if (retry && result.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        throw new ApiException($"Unauthorized {url}", result.StatusCode);
                    }
                    var error = await result.Content?.ReadAsStringAsync() ?? string.Empty;
                    _logger.LogInformation(" === API === " + url + " => " + (string.IsNullOrEmpty(json) ? "" : json) + " " + error);
                    throw new ApiException(error, result.StatusCode);
                }
            }
            catch
            {
                throw;
            }
        }

        private async Task<TOut> _GetAsync<TOut, TIn>(string url, TIn query, bool authRequired, bool isPlainText = false, bool retry = true, CancellationToken? cancellationToken = null) where TIn : class where TOut : class
        {
            var token = cancellationToken ?? CancellationToken.None;
            var _url = url;
            if (typeof(TIn) == typeof(Void))
            {
                _url += ConvertToQuery(query);
            }

            try
            {
                var checkInternet = GenericUtilities.CheckInternetConnection();
                if (!checkInternet)
                    throw new Exception("No internet connection");
                using (var client = _httpClientFactory.CreateClient("api"))
                {
                    if (authRequired)
                    {
                        //  var bearer = await SecureStorage.GetAsync("access_token");
                        //client.DefaultRequestHeaders.Add($"Authorization", $"Bearer {bearer}");
                    }

                    if (typeof(TIn) == typeof(Void))
                    {
                        url += ConvertToQuery(query);
                    }

                    if (typeof(TOut) == typeof(byte[]))
                    {
                        var resultByteArray = await client.GetByteArrayAsync(url, token);
                        return resultByteArray as TOut;
                    }
                    else
                    {
                        var result = await client.GetAsync(url, token);
                        if (result.IsSuccessStatusCode)
                        {
                            if (typeof(TOut) == typeof(Void))
                            {
                                return Void.Empty as TOut;
                            }
                            else
                            {
                                if (result.StatusCode == System.Net.HttpStatusCode.NoContent)
                                {
                                    var nocontent = Activator.CreateInstance<TOut>();
                                    return nocontent;
                                }

                                var response = await result.Content.ReadAsStringAsync();
#if DEBUG
                                Console.WriteLine($"RESPONSE FROM {url} #####\n{response}");
#endif
                                if (isPlainText)
                                {
                                    response = JsonSerializer.Serialize(response);
                                }
                                var returnValue = JsonSerializer.Deserialize<TOut>(response, new JsonSerializerOptions
                                {
                                    PropertyNameCaseInsensitive = true
                                });
                                return returnValue;
                            }
                        }


                        if (retry && result.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                        {
                            throw new ApiException($"Unauthorized {url}", result.StatusCode);
                        }
                        var error = await result.Content?.ReadAsStringAsync() ?? string.Empty;
                        throw new ApiException(error, result.StatusCode);
                    }
                }
            }
            catch (TaskCanceledException) { return default(TOut); }
            catch (Exception ex)
            {
                throw;
            }
        }


        private string ConvertToQuery<T>(T Body) where T : class
        {
            if (Body == null)
                return string.Empty;
            if (Body.GetType() == typeof(string))
                return Body.ToString();

            var query = string.Empty;
            foreach (var p in Body.GetType().GetProperties().Where(pi => Attribute.IsDefined(pi, typeof(Form))))
            {
                var form = ((Form)p.GetCustomAttributes(typeof(Form), false)[0]).label;
                var value = p.GetValue(Body).ToString();
                query += $"{form}={value}&";

            }
            return query == string.Empty ? query : $"?{query}";

        }

    }

    public class Void
    {
        private static Void empty;
        public static Void Empty => empty ?? (empty = new Void());
        public static string NotSerialize;
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class Form : Attribute
    {
        public string label { get; set; }
        public Form(string label)
        {
            this.label = label;
        }
    }

    public class ApiException : Exception
    {
        public HttpStatusCode StatusCode { get; set; }
        public ApiException(string message, HttpStatusCode statusCode) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
