namespace DecissionService.Core.Domain
{
    public class Address
    {
        //public Address() => User = new User();

        public int Id { get; set; }
        public string UserId { get; set; }
        public virtual User User { get; set; }
        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public string? ZipCode { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
