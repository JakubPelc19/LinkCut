namespace LinkCut.Models
{
    public class ShortLink
    {
        public int Id { get; set; }
        
        // Link provided by client
        public Uri OriginalLink { get; set; }
      
        public string OriginalLinkId { get; set; }
    }
}
