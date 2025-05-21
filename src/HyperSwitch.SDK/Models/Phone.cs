using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace HyperSwitch.SDK.Models
{
    /// <summary>
    /// Represents a phone number with country code
    /// </summary>
    public class Phone
    {
        /// <summary>
        /// Phone number
        /// </summary>
        [JsonProperty("number")]
        [JsonPropertyName("number")]
        public string? Number { get; set; }

        /// <summary>
        /// Country code with + prefix (e.g., "+1", "+44", etc.)
        /// </summary>
        [JsonProperty("country_code")]
        [JsonPropertyName("country_code")]
        public string? CountryCode { get; set; }
    }
} 