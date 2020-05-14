
using Newtonsoft.Json;

namespace IdentityService.Core.Domain.DTOs
{
    public class UserDto
    {
        [JsonProperty(propertyName: "firstName")]
        public string? FirstName { get; set; }

        [JsonProperty(propertyName: "lastName")]
        public string? LastName { get; set; }
    }
}
