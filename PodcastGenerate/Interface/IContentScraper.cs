namespace PodcastGenerate.Interface
{
    public interface IContentScraper
    {
        Task<string> ScrapeContentAsync(string url);
    }
}
