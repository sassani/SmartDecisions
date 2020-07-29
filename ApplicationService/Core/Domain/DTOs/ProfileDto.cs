using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ApplicationService.Core.Domain.DTOs
{
    public class ProfileDto
    {
        [JsonPropertyName("firstName")]
        public string? FirstName { get; set; } = default!;

        [JsonPropertyName("lastName")]
        public string? LastName { get; set; } = default!;

        [JsonPropertyName("avatar")]
        public AvatarDto? Avatar { get; set; }

        [JsonPropertyName("company")]
        public string? Company { get; set; }

        [JsonPropertyName("contacts")]
        public virtual ICollection<ContactDto>? Contacts { get; set; }
    }
}
