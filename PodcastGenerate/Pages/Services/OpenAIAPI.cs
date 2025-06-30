using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using PodcastGenerate.Models.Configurations;
using ChatMessage = OpenAI.Chat.ChatMessage;
using OpenAI.Chat;
using OpenAI;
using Azure.AI.OpenAI;
namespace PodcastGenerate.Pages.Classes
{
	public class OpenAIAPI
	{
		private readonly AzureOpenAIClient _openAIClient;
		private readonly HttpClient _httpClient;
		private readonly AzureOpenAIConfig _config;
		private readonly ChatClient _chatClient;
		public OpenAIAPI(IOptions<AzureOpenAIConfig> config)
		{
			_config = config.Value ?? throw new ArgumentNullException(nameof(config));
			_httpClient = new HttpClient();
			_httpClient.DefaultRequestHeaders.Authorization =
				new AuthenticationHeaderValue("Bearer", _config.ApiKey);
			//_chatClient = _openAIClient.GetChatClient(deploymentName);
			// Set default timeout
			_httpClient.Timeout = TimeSpan.FromSeconds(30);
		}
		public async Task<string> SummarizeBlog(string blogContent)
		{
			List<ChatMessage> messages = new List<ChatMessage>()
		 {
		 	new SystemChatMessage("Summarize the following text in 2000 characters for podcast script..."),
		 	new UserChatMessage(blogContent),
			};

			var response = await _chatClient.CompleteChatAsync(messages);
			return response.Value.Content[0].Text;
		}
		//public ChatEndpoint Chat => new ChatEndpoint(_httpClient, _config);

		//public class ChatEndpoint
		//{
		//	private readonly HttpClient _httpClient;
		//	private readonly AzureOpenAIConfig _config;

		//	public ChatEndpoint(HttpClient httpClient, AzureOpenAIConfig config)
		//	{
		//		_httpClient = httpClient;
		//		_config = config;
		//	}

		//	public async Task<ChatCompletionResult> CreateChatCompletionAsync(ChatRequest request)
		//	{
		//		if (string.IsNullOrEmpty(_config.Endpoint))
		//		{
		//			throw new InvalidOperationException("OpenAI endpoint is not configured");
		//		}

		//		var content = new StringContent(
		//			JsonSerializer.Serialize(request),
		//			Encoding.UTF8,
		//			"application/json");

		//		var response = await _httpClient.PostAsync(_config.Endpoint, content);

		//		if (!response.IsSuccessStatusCode)
		//		{
		//			var errorContent = await response.Content.ReadAsStringAsync();
		//			throw new HttpRequestException($"OpenAI API request failed: {response.StatusCode} - {errorContent}");
		//		}

		//		var responseJson = await response.Content.ReadAsStringAsync();
		//		return JsonSerializer.Deserialize<ChatCompletionResult>(responseJson);
		//	}
		//}

	}
}
