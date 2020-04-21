using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace UserService.Core.Domain.DTOs
{
    public class AddressDto
    {
        [JsonPropertyName("id")]
        [JsonRequired]
        public int Id { get; set; }
        [JsonPropertyName("address1")]
        public string? Address1 { get; set; }
        [JsonPropertyName("address2")]
        public string? Address2 { get; set; }
        [JsonPropertyName("city")]
        public string? City { get; set; }
        [JsonPropertyName("state")]
        public string? State { get; set; }
        [JsonPropertyName("country")]
        public string? Country { get; set; }
        [JsonPropertyName("zipcode")]
        public string? Zipcode { get; set; }
        [JsonPropertyName("phoneNumber")]
        public string? PhoneNumber { get; set; }
    }
}
