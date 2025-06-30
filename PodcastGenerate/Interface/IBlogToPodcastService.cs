namespace PodcastGenerate.Interface
{
    public interface IBlogToPodcastService
    {
        Task<byte[]> ConvertBlogToPodcastAsync(string blogUrl);
    }
}
