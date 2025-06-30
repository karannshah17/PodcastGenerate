namespace PodcastGenerate.Interface
{
    public interface IContentSummarizer
    {
        Task<string> SummarizeAsync(string content);
    }
}
