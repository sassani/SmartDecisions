namespace ApplicationService.Core.Domain
{
    public class ShareableResource : Owner
    {
        public string? SharedWith { get; set; }
    }
}
