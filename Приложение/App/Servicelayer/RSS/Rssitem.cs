namespace DataLayer.RSS
{
    public class RssItem
    {
        public required string Title { get; set; }
        public string? Image { get; set; }
        public string? Description { get; set; }
        public DateTime PublicationDate { get; set; }
    }
}
