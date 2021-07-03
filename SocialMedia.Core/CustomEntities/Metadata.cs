namespace SocialMedia.Core.CustomEntities
{
    public class Metadata
    {
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public bool HasNextPage { get; set; }
        public bool HasPreviuosPage { get; set; }
        public string NextPageUrl { get; set; }
        public string PreviuosPageUrl { get; set; }
    }
}
