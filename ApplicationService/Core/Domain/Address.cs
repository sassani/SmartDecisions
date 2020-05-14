using System.ComponentModel.DataAnnotations;

namespace ApplicationService.Core.Domain
{
    public class Address : Owner
    {
        public int Id { get; set; }
        [MaxLength(36)]
        public string ProfileId { get; set; }
        public virtual Profile Profile { get; set; }
        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public string? ZipCode { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
