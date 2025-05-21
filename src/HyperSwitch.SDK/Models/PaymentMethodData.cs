using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace HyperSwitch.SDK.Models
{
    /// <summary>
    /// Represents payment method data for different payment methods
    /// </summary>
    public class PaymentMethodData
    {
        /// <summary>
        /// Card payment method details
        /// </summary>
        [JsonProperty("card")]
        [JsonPropertyName("card")]
        public CardDetails? Card { get; set; }

        // Other payment methods can be added here as needed
    }
} 