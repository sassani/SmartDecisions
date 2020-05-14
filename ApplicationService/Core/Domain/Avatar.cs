namespace ApplicationService.Core.Domain
{
    public class Avatar:FileObject
    {
        public virtual Profile Profile { get; set; }
        public string ProfileId { get; set; }
    }
}
