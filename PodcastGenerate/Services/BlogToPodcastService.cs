using PodcastGenerate.Interface;
using System.Runtime;

namespace PodcastGenerate.Services
{
    public class BlogToPodcastService : IBlogToPodcastService
    {
        private readonly IContentScraper _scraper;
        private readonly IContentSummarizer _summarizer;
        private readonly ITextToSpeechConverter _ttsConverter;
        private readonly ILogger<BlogToPodcastService> _logger;
        // Constructor injection
        public BlogToPodcastService(
           IContentScraper scraper,
           IContentSummarizer summarizer,
           ITextToSpeechConverter ttsConverter, ILogger<BlogToPodcastService> logger)
        {
            _scraper = scraper;
            _summarizer = summarizer;
            _ttsConverter = ttsConverter;
            _logger = logger;
        }
        public async Task<byte[]> ConvertBlogToPodcastAsync(string blogUrl)
        {
            try
            {
                var scrapedContent = await _scraper.ScrapeContentAsync(blogUrl);
                if (string.IsNullOrWhiteSpace(scrapedContent))
                    _logger.LogError("No content found at the provided URL.");
               
                var summary = await _summarizer.SummarizeAsync(
                    scrapedContent
               );
                if (string.IsNullOrWhiteSpace(summary))
                    _logger.LogError("Summarization failed");
                var audioBytes = await _ttsConverter.ConvertToSpeechAsync(
                    summary
                    );
               
                if (audioBytes == null || audioBytes.Length == 0)
                    _logger.LogError("Audio generation failed.");
                return audioBytes;
            }
            catch(Exception ex)
            {
                 _logger.LogError("Failed to generate podcast audio.", ex);
                return null;
            }
        }



    }
}
