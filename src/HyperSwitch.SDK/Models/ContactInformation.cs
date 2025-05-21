using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace HyperSwitch.SDK.Models
{
    /// <summary>
    /// Represents contact information including address and phone
    /// </summary>
    public class ContactInformation
    {
        /// <summary>
        /// Address information
        /// </summary>
        [JsonProperty("address")]
        [JsonPropertyName("address")]
        public Address? Address { get; set; }

        /// <summary>
        /// Phone information
        /// </summary>
        [JsonProperty("phone")]
        [JsonPropertyName("phone")]
        public Phone? Phone { get; set; }
    }
} 