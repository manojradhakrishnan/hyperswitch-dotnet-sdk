using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace HyperSwitch.SDK.Models
{
    /// <summary>
    /// Represents a billing or shipping address
    /// </summary>
    public class Address
    {
        /// <summary>
        /// First line of the address
        /// </summary>
        [JsonProperty("line1")]
        [JsonPropertyName("line1")]
        public string? Line1 { get; set; }

        /// <summary>
        /// Second line of the address
        /// </summary>
        [JsonProperty("line2")]
        [JsonPropertyName("line2")]
        public string? Line2 { get; set; }

        /// <summary>
        /// Third line of the address
        /// </summary>
        [JsonProperty("line3")]
        [JsonPropertyName("line3")]
        public string? Line3 { get; set; }

        /// <summary>
        /// City
        /// </summary>
        [JsonProperty("city")]
        [JsonPropertyName("city")]
        public string? City { get; set; }

        /// <summary>
        /// State/Province/Region
        /// </summary>
        [JsonProperty("state")]
        [JsonPropertyName("state")]
        public string? State { get; set; }

        /// <summary>
        /// ZIP/Postal code
        /// </summary>
        [JsonProperty("zip")]
        [JsonPropertyName("zip")]
        public string? Zip { get; set; }

        /// <summary>
        /// Country code (e.g., "US", "GB", etc.)
        /// </summary>
        [JsonProperty("country")]
        [JsonPropertyName("country")]
        public string? Country { get; set; }

        /// <summary>
        /// First name of the recipient
        /// </summary>
        [JsonProperty("first_name")]
        [JsonPropertyName("first_name")]
        public string? FirstName { get; set; }

        /// <summary>
        /// Last name of the recipient
        /// </summary>
        [JsonProperty("last_name")]
        [JsonPropertyName("last_name")]
        public string? LastName { get; set; }
    }
} 