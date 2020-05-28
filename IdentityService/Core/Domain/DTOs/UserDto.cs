
using System.Text.Json.Serialization;

namespace IdentityService.Core.Domain.DTOs
{
    public class UserDto
    {
        [JsonPropertyName("firstName")]
        public string? FirstName { get; set; }

        [JsonPropertyName("lastName")]
        public string? LastName { get; set; }
    }
}
