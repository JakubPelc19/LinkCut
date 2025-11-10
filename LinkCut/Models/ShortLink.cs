namespace LinkCut.Models
{
    public class ShortLink
    {
        public int Id { get; set; }
        
        // Link provided by client
        public string OriginalLink { get; set; }
        // Short version of OriginalLink
        public string OriginalLinkId { get; set; }
    }
}
