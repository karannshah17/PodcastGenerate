using Azure;
using Azure.AI.OpenAI;
using Microsoft.Extensions.Options;
using OpenAI;
using OpenAI.Chat;
using PodcastGenerate.Interface;
using PodcastGenerate.Models.Configurations;

namespace PodcastGenerate.Services
{
    public class AzureOpenAISummarizer : IContentSummarizer
    {
        private readonly ChatClient _chatClient;
        private readonly AzureOpenAIConfig _config;
        private readonly ILogger<AzureOpenAISummarizer> _logger;

        public AzureOpenAISummarizer(IOptions<AzureOpenAIConfig> config, ILogger<AzureOpenAISummarizer> logger)
        {
            _config = config.Value ?? throw new ArgumentNullException(nameof(config));
            _logger = logger;
            // Validate configuration
            if (string.IsNullOrWhiteSpace(_config.ApiKey))
                _logger.LogError("OpenAI API key is required");

            if (string.IsNullOrWhiteSpace(_config.Endpoint))
                _logger.LogError("OpenAI endpoint is required");

            // Initialize Azure OpenAI client


            var openAIClient = new AzureOpenAIClient(
                new Uri(_config.Endpoint),
                new AzureKeyCredential(_config.ApiKey));

            _chatClient = openAIClient.GetChatClient(_config.Model);
        }
        public async Task<string> SummarizeAsync(string content)
        {
            try
            {
                var messages = new List<ChatMessage>()
                {
                    new SystemChatMessage(_config.Prompt),
                    new UserChatMessage(content),
                };
                
                var response = await _chatClient.CompleteChatAsync(messages);
                if (response?.Value?.Content == null || response.Value.Content.Count == 0)
                {
                    _logger.LogWarning("Summarization response was empty or null.");
                    return null; // or string.Empty, as appropriate
                }
                return response.Value.Content[0].Text;
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while summarizing content.");
                return null; // or string.Empty, as appropriate for your use case
            }
        }

    }
}
