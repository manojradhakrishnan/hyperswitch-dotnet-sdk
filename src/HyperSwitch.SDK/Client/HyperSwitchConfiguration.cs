using System;
using System.Net.Http;

namespace HyperSwitch.SDK.Client
{
    /// <summary>
    /// Configuration class for HyperSwitch API client
    /// </summary>
    public class HyperSwitchConfiguration
    {
        /// <summary>
        /// The base URL for the HyperSwitch API
        /// </summary>
        public string BaseUrl { get; set; } = "https://sandbox.hyperswitch.io";

        /// <summary>
        /// The API key for authenticating with the HyperSwitch API
        /// </summary>
        public string ApiKey { get; set; }

        /// <summary>
        /// Optional custom HTTP client
        /// </summary>
        public HttpClient? HttpClient { get; set; }

        /// <summary>
        /// Timeout for API requests (default: 100 seconds)
        /// </summary>
        public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(100);

        /// <summary>
        /// Creates a new instance of the HyperSwitchConfiguration class
        /// </summary>
        public HyperSwitchConfiguration()
        {
        }

        /// <summary>
        /// Creates a new instance of the HyperSwitchConfiguration class with API key
        /// </summary>
        /// <param name="apiKey">The API key for authenticating with the HyperSwitch API</param>
        public HyperSwitchConfiguration(string apiKey)
        {
            ApiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));
        }

        /// <summary>
        /// Creates a new instance of the HyperSwitchConfiguration class with API key and base URL
        /// </summary>
        /// <param name="apiKey">The API key for authenticating with the HyperSwitch API</param>
        /// <param name="baseUrl">The base URL for the HyperSwitch API</param>
        public HyperSwitchConfiguration(string apiKey, string baseUrl)
        {
            ApiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));
            BaseUrl = baseUrl ?? throw new ArgumentNullException(nameof(baseUrl));
        }
    }
} 