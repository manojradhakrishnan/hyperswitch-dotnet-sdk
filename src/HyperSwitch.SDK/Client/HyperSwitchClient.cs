using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using HyperSwitch.SDK.Api;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace HyperSwitch.SDK.Client
{
    /// <summary>
    /// Client for HyperSwitch API
    /// </summary>
    public class HyperSwitchClient : IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly HyperSwitchConfiguration _configuration;
        private readonly bool _disposeHttpClient;
        private readonly JsonSerializerOptions _jsonOptions;

        /// <summary>
        /// Payment API endpoints
        /// </summary>
        public PaymentsApi Payments { get; }

        /// <summary>
        /// Whether to enable detailed logging of requests and responses
        /// </summary>
        public bool EnableDetailedLogging { get; set; } = false;

        /// <summary>
        /// Event that's triggered whenever a request is made
        /// </summary>
        public event EventHandler<RequestEventArgs> RequestSent;

        /// <summary>
        /// Event that's triggered whenever a response is received
        /// </summary>
        public event EventHandler<ResponseEventArgs> ResponseReceived;

        /// <summary>
        /// Initializes a new instance of the HyperSwitchClient class
        /// </summary>
        /// <param name="configuration">Configuration for the client</param>
        public HyperSwitchClient(HyperSwitchConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            
            if (string.IsNullOrEmpty(configuration.ApiKey))
            {
                throw new ArgumentException("API key must be provided in the configuration", nameof(configuration));
            }

            if (configuration.HttpClient != null)
            {
                _httpClient = configuration.HttpClient;
                _disposeHttpClient = false;
            }
            else
            {
                _httpClient = new HttpClient();
                _httpClient.Timeout = configuration.Timeout;
                _disposeHttpClient = true;
            }

            _httpClient.BaseAddress = new Uri(configuration.BaseUrl);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.Add("api-key", configuration.ApiKey);

            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = false
            };
            
            // Initialize API interfaces
            Payments = new PaymentsApi(this);
        }

        /// <summary>
        /// Sends a GET request to the specified URL
        /// </summary>
        /// <typeparam name="T">The type of response to deserialize to</typeparam>
        /// <param name="path">The API endpoint path</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The API response</returns>
        public async Task<T> GetAsync<T>(string path, CancellationToken cancellationToken = default)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, path);
            LogRequest(request, null);
            
            var response = await _httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);
            return await ProcessResponseAsync<T>(response, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Sends a POST request to the specified URL
        /// </summary>
        /// <typeparam name="T">The type of response to deserialize to</typeparam>
        /// <param name="path">The API endpoint path</param>
        /// <param name="data">The request data to send</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The API response</returns>
        public async Task<T> PostAsync<T>(string path, object data, CancellationToken cancellationToken = default)
        {
            var jsonString = JsonConvert.SerializeObject(data, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
            
            var content = new StringContent(
                jsonString,
                Encoding.UTF8,
                "application/json"
            );

            var request = new HttpRequestMessage(HttpMethod.Post, path) { Content = content };
            LogRequest(request, jsonString);
            
            var response = await _httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);
            return await ProcessResponseAsync<T>(response, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Sends a PUT request to the specified URL
        /// </summary>
        /// <typeparam name="T">The type of response to deserialize to</typeparam>
        /// <param name="path">The API endpoint path</param>
        /// <param name="data">The request data to send</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The API response</returns>
        public async Task<T> PutAsync<T>(string path, object data, CancellationToken cancellationToken = default)
        {
            var jsonString = JsonConvert.SerializeObject(data, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
            
            var content = new StringContent(
                jsonString,
                Encoding.UTF8,
                "application/json"
            );

            var request = new HttpRequestMessage(HttpMethod.Put, path) { Content = content };
            LogRequest(request, jsonString);
            
            var response = await _httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);
            return await ProcessResponseAsync<T>(response, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Processes an HTTP response
        /// </summary>
        /// <typeparam name="T">The type to deserialize the response to</typeparam>
        /// <param name="response">The HTTP response</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The deserialized response</returns>
        private async Task<T> ProcessResponseAsync<T>(HttpResponseMessage response, CancellationToken cancellationToken)
        {
            var content = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
            LogResponse(response, content);

            if (!response.IsSuccessStatusCode)
            {
                throw new HyperSwitchApiException(
                    $"API request failed with status code {(int)response.StatusCode}: {response.ReasonPhrase}",
                    (int)response.StatusCode,
                    content,
                    response.Headers
                );
            }

            return JsonConvert.DeserializeObject<T>(content);
        }

        /// <summary>
        /// Logs request details if logging is enabled
        /// </summary>
        private void LogRequest(HttpRequestMessage request, string content)
        {
            if (EnableDetailedLogging)
            {
                Console.WriteLine($"\n[REQUEST] {request.Method} {request.RequestUri}");
                Console.WriteLine("[REQUEST HEADERS]");
                foreach (var header in request.Headers)
                {
                    if (header.Key != "api-key") // Don't log API key
                    {
                        Console.WriteLine($"  {header.Key}: {string.Join(", ", header.Value)}");
                    }
                    else
                    {
                        Console.WriteLine($"  {header.Key}: [REDACTED]");
                    }
                }
                
                if (!string.IsNullOrEmpty(content))
                {
                    try
                    {
                        // Try to format the JSON for better readability
                        var deserializedJson = JsonConvert.DeserializeObject(content);
                        var formattedJson = JsonConvert.SerializeObject(deserializedJson, Formatting.Indented);
                        Console.WriteLine("[REQUEST BODY]");
                        Console.WriteLine(formattedJson);
                    }
                    catch
                    {
                        Console.WriteLine("[REQUEST BODY]");
                        Console.WriteLine(content);
                    }
                }
            }
            
            RequestSent?.Invoke(this, new RequestEventArgs(request, content));
        }

        /// <summary>
        /// Logs response details if logging is enabled
        /// </summary>
        private void LogResponse(HttpResponseMessage response, string content)
        {
            if (EnableDetailedLogging)
            {
                Console.WriteLine($"\n[RESPONSE] {(int)response.StatusCode} {response.ReasonPhrase}");
                Console.WriteLine("[RESPONSE HEADERS]");
                foreach (var header in response.Headers)
                {
                    Console.WriteLine($"  {header.Key}: {string.Join(", ", header.Value)}");
                }
                
                if (!string.IsNullOrEmpty(content))
                {
                    try
                    {
                        // Try to format the JSON for better readability
                        var deserializedJson = JsonConvert.DeserializeObject(content);
                        var formattedJson = JsonConvert.SerializeObject(deserializedJson, Formatting.Indented);
                        Console.WriteLine("[RESPONSE BODY]");
                        Console.WriteLine(formattedJson);
                    }
                    catch
                    {
                        Console.WriteLine("[RESPONSE BODY]");
                        Console.WriteLine(content);
                    }
                }
            }
            
            ResponseReceived?.Invoke(this, new ResponseEventArgs(response, content));
        }

        /// <summary>
        /// Disposes the HTTP client if it was created by this class
        /// </summary>
        public void Dispose()
        {
            if (_disposeHttpClient)
            {
                _httpClient?.Dispose();
            }
        }
    }

    /// <summary>
    /// Event arguments for a request
    /// </summary>
    public class RequestEventArgs : EventArgs
    {
        /// <summary>
        /// The HTTP request message
        /// </summary>
        public HttpRequestMessage Request { get; }
        
        /// <summary>
        /// The request content
        /// </summary>
        public string Content { get; }
        
        /// <summary>
        /// Creates a new instance of RequestEventArgs
        /// </summary>
        public RequestEventArgs(HttpRequestMessage request, string content)
        {
            Request = request;
            Content = content;
        }
    }

    /// <summary>
    /// Event arguments for a response
    /// </summary>
    public class ResponseEventArgs : EventArgs
    {
        /// <summary>
        /// The HTTP response message
        /// </summary>
        public HttpResponseMessage Response { get; }
        
        /// <summary>
        /// The response content
        /// </summary>
        public string Content { get; }
        
        /// <summary>
        /// Creates a new instance of ResponseEventArgs
        /// </summary>
        public ResponseEventArgs(HttpResponseMessage response, string content)
        {
            Response = response;
            Content = content;
        }
    }

    /// <summary>
    /// Exception thrown when an API request fails
    /// </summary>
    public class HyperSwitchApiException : Exception
    {
        /// <summary>
        /// HTTP status code of the response
        /// </summary>
        public int StatusCode { get; }

        /// <summary>
        /// Response body as string
        /// </summary>
        public string ResponseBody { get; }

        /// <summary>
        /// Response headers
        /// </summary>
        public HttpResponseHeaders Headers { get; }

        /// <summary>
        /// Creates a new instance of the HyperSwitchApiException class
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="statusCode">HTTP status code</param>
        /// <param name="responseBody">Response body</param>
        /// <param name="headers">Response headers</param>
        public HyperSwitchApiException(string message, int statusCode, string responseBody, HttpResponseHeaders headers)
            : base(message)
        {
            StatusCode = statusCode;
            ResponseBody = responseBody;
            Headers = headers;
        }
    }
} 