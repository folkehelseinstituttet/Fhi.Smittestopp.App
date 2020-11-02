using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using NDB.Covid19.Models;
using NDB.Covid19.WebServices.ErrorHandlers;
using NDB.Covid19.WebServices.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace NDB.Covid19.WebServices
{
    public class BaseWebService
    {
        HttpClientManager _httpClientManager
        {
            get
            {
                HttpClientManager manager = HttpClientManager.Instance;
                manager.AddSecretToHeaderIfMissing();
                return manager;
            }
        }

        HttpClient _client => _httpClientManager.HttpClientAccessor.HttpClient;
        readonly BadConnectionErrorHandler _badConnectionErrorHandler = new BadConnectionErrorHandler();

        public async Task<ApiResponse<M>> Get<M>(String url)
        {
            var result = await InnerGet<M>(url);

            if (result.IsSuccessfull == false)
            {
                // If it's because of bad connection, retry again
                if (_badConnectionErrorHandler.IsResponsible(result))
                {
                    Debug.WriteLine("Sending for the second time because of bad connection: " + url);
                    result = await InnerGet<M>(url);
                }
            }

            return result;
        }

        async Task<ApiResponse<M>> InnerGet<M>(string url)
        {
            ApiResponse<M> result = new ApiResponse<M>(url, HttpMethod.Get);
            try
            {
                HttpResponseMessage response = await _client.GetAsync(url);
                result.StatusCode = (int)response.StatusCode;
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    if (!string.IsNullOrEmpty(content))
                    {
                        result.ResponseText = content;
                        MapData<M>(result, content);
                    }

                    Debug.WriteLine("Page content: " + content);
                }
                else
                {
                    result.ResponseText = response.ReasonPhrase;
                }
            }
            catch (Exception e)
            {
                result.Exception = e;
                Debug.WriteLine(@"\tERROR {0}", e.Message);
            }
            return result;
        }

        virtual public async Task<ApiResponse<Stream>> GetFileAsStreamAsync(String url)
        {
            Debug.WriteLine($"Using URL: {url}");
            ApiResponse<Stream> result = new ApiResponse<Stream>(url, HttpMethod.Get);
            try
            {
                // Uncomment to disable cache and get all keys up to this point
                //_client.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue { NoCache = true };
                HttpResponseMessage response = await _client.GetAsync(url);

                result.StatusCode = (int) response.StatusCode;

                if (response.IsSuccessStatusCode)
                {
                    Stream content = await response.Content.ReadAsStreamAsync();
                    result.Headers = response.Headers;
                    if (content.Length > 0)
                    {
                        result.Data = content;
                        PrintFileSize(response);
                    }
                }
                else
                {
                    result.ResponseText = response.ReasonPhrase;
                }
            }
            catch (Exception e)
            {
                result.Exception = e;
                Debug.WriteLine(@"\tERROR {0}", e.Message);
            }
            return result;
        }

        void PrintFileSize(HttpResponseMessage response)
        {
#if DEBUG
            Debug.WriteLine("Downloaded file with size " + response.Content.Headers.GetValues("Content-Length").First() + " bytes");
#endif
        }

        public async Task<ApiResponse> Post(String url)
        {
            Debug.WriteLine(@"\Post method, empty body, start.");
            return await Post<object>(null, url);
        }

        public virtual async Task<ApiResponse> Post<T>(T t, String url)
        {
            string jsonBody = JsonConvert.SerializeObject(t, JsonSerializerSettings);
            StringContent content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            Debug.WriteLine("Sending json " + content.ReadAsStringAsync().Result);

            ApiResponse apiResponse =  await InnerPost(content, url);

            if (apiResponse.IsSuccessfull == false)
            {

                // If it's because of bad connection, retry again
                if (_badConnectionErrorHandler.IsResponsible(apiResponse))
                {
                    Debug.WriteLine("Sending for second time because of bad connection: " + url);
                    apiResponse = await InnerPost(content, url);
                }
            }
  
            return apiResponse;

        }

        //content might be null
        async Task<ApiResponse> InnerPost(StringContent content, String url)
        {
            ApiResponse result = new ApiResponse(url, HttpMethod.Post);
            try
            {
                HttpResponseMessage response = await _client.PostAsync(url, content);
                result.StatusCode = (int)response.StatusCode;
                PrintHeadersToConsole(response);

                if (response.IsSuccessStatusCode)
                {
                    result.ResponseText = await response.Content.ReadAsStringAsync();

                    Debug.WriteLine("Response status: " + result.StatusCode + " responseText: " +
                                    result.ResponseText);
                }
                else
                {
                    Debug.Print(response.StatusCode.ToString());
                    Debug.Print(response.ReasonPhrase);

                    try
                    {
                        result.ResponseText = response.ReasonPhrase;
                    }
                    catch (Exception) { }
                }
            }
            catch (Exception e)
            {
                result.Exception = e;
            }
            return result;
        }

        void PrintHeadersToConsole(HttpResponseMessage response)
        {
            try
            {
                if (response.RequestMessage?.Headers != null)
                {
                    Debug.Print("Request headers:");
                    Debug.Print("-----");
                    foreach (var header in response.RequestMessage?.Headers)
                    {
                        string value = string.Join("; ", header.Value);
                        Debug.Print(header.Key + ": " + value);
                    }
                    Debug.Print("-----");
                }
            }
            catch (Exception e)
            {
                Debug.Print("Failed to print headers to console.");
                Debug.Print(e.ToString());
            }
        }

        public static JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            DateTimeZoneHandling = DateTimeZoneHandling.Local
        };
        public static JsonSerializer JsonSerializer = new JsonSerializer();

        public void MapData<M>(ApiResponse<M> resultObj, string content)
        {
            JObject obj = JObject.Parse(content);
            if (obj != null)
            {
                resultObj.Data = obj.ToObject<M>(JsonSerializer);
            }

        }

        /// <summary>
        /// Contains a set of default error handlers which might interupt or block the UI with an error message.
        /// The first error handler which is responsible of a given error will handle the error. The rest will be ignored.
        /// If there are no errors, then nothing will happen.
        /// </summary>
        /// <param name="extraErrorHandlers">Extra error handlers to add in front of the default ones</param>
        public static void HandleErrors(ApiResponse response, params IErrorHandler[] extraErrorHandlers)
        {
            HandleErrors(response, false, extraErrorHandlers);
        }
        /// <summary>
        /// Contains a set of default error handlers which are not interrupting the user.
        /// The first error handler which is responsible of a given error will handle the error. The rest will be ignored.
        /// If there are no errors, then nothing will happen.
        /// </summary>
        /// <param name="extraErrorHandlers">Extra error handlers to add in front of the default ones</param>
        public static void HandleErrorsSilently(ApiResponse response, params IErrorHandler[] extraErrorHandlers)
        {
            HandleErrors(response, true, extraErrorHandlers);
        }
        static void HandleErrors(ApiResponse response, bool silently, params IErrorHandler[] extraErrorHandlers)
        {
            //Check these error handlers before any custom error handlers
            List<IErrorHandler> errorHandlersAlwaysCheckFirst = new List<IErrorHandler>
            {
                new ApiDeprecatedErrorHandler(),
                new NoInternetErrorHandler(silently),
                new BadConnectionErrorHandler(silently),
                new TimeoutErrorHandler(silently)
            };
            //Check these error handlers after any custom error handlers
            List<IErrorHandler> errorHandlersCheckAfterCustom = new List<IErrorHandler>
            {
                new DefaultErrorHandler(silently)
            };
            List<IErrorHandler> errorHandlers = errorHandlersAlwaysCheckFirst
                .Concat(extraErrorHandlers)
                .Concat(errorHandlersCheckAfterCustom)
                .ToList();
            Handle(response, errorHandlers);
        }

        static void Handle(ApiResponse response, List<IErrorHandler> errorHandlers)
        {
            foreach (IErrorHandler errorHandler in errorHandlers)
            {
                if (errorHandler.IsResponsible(response))
                {
                    errorHandler.HandleError(response);
                    return;
                }
            }
        }
    }
}
