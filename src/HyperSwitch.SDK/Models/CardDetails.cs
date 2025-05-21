using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace HyperSwitch.SDK.Models
{
    /// <summary>
    /// Represents card payment method details
    /// </summary>
    public class CardDetails
    {
        /// <summary>
        /// Card number
        /// </summary>
        [JsonProperty("card_number")]
        [JsonPropertyName("card_number")]
        public string? CardNumber { get; set; }

        /// <summary>
        /// Card expiration month (2 digits)
        /// </summary>
        [JsonProperty("card_exp_month")]
        [JsonPropertyName("card_exp_month")]
        public string? CardExpMonth { get; set; }

        /// <summary>
        /// Card expiration year (2 or 4 digits)
        /// </summary>
        [JsonProperty("card_exp_year")]
        [JsonPropertyName("card_exp_year")]
        public string? CardExpYear { get; set; }

        /// <summary>
        /// Name of the card holder
        /// </summary>
        [JsonProperty("card_holder_name")]
        [JsonPropertyName("card_holder_name")]
        public string? CardHolderName { get; set; }

        /// <summary>
        /// Card verification code (CVV/CVC)
        /// </summary>
        [JsonProperty("card_cvc")]
        [JsonPropertyName("card_cvc")]
        public string? CardCvc { get; set; }
    }
} 