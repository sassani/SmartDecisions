
using Newtonsoft.Json;

namespace RestApi.Core.Domain.DTOs
{
    public class RegisterUserDto
    {
        [JsonProperty(propertyName: "firstName")]
        public string FirstName { get; set; }

        [JsonProperty(propertyName: "lastName")]
        public string LastName { get; set; }

        [JsonProperty(propertyName: "email")]
        public string Email { get; set; }

        [JsonProperty(propertyName: "password")]
        public string Password { get; set; }
    }
}
