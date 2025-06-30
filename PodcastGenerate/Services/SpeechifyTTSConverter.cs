using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PodcastGenerate.Interface;
using PodcastGenerate.Models;
using PodcastGenerate.Models.Configurations;
using RestSharp;
using System.Text.Json;
namespace PodcastGenerate.Services
{

    public class SpeechifyTTSConverter : ITextToSpeechConverter
    {
        private readonly RestClient _client;
        private readonly SpeechifyConfig _config;
        private readonly ILogger<SpeechifyTTSConverter> _logger;
        public SpeechifyTTSConverter(IOptions<SpeechifyConfig> config, ILogger<SpeechifyTTSConverter> logger)
        {
            _config = config.Value ?? throw new ArgumentNullException(nameof(config));

            if (string.IsNullOrWhiteSpace(_config.ApiKey))
                throw new ArgumentException("API key is required");

            if (string.IsNullOrWhiteSpace(_config.Endpoint))
                throw new ArgumentException("Endpoint is required");

            //var options = new RestClientOptions(_config.Endpoint)
            //{

            //    ThrowOnAnyError = false
            //};

            _client = new RestClient(_config.Endpoint);
            _logger = logger;
        }

        public async Task<byte[]> ConvertToSpeechAsync(string text)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(text))
                    _logger.LogWarning("Text cannot be null or empty");

                var request = new RestRequest
                {
                    Method = Method.Post
                };

                request.AddHeader("Authorization", $"Bearer {_config.ApiKey}");
                request.AddHeader("Content-Type", "application/json");
                request.AddJsonBody(new
                {
                    input = text,
                    voice_id = _config.DefaultVoiceId
                });

                var response = await _client.ExecuteAsync(request);

                if (!response.IsSuccessful)
                {

                    _logger.LogError(
                        $"TTS API request failed: {response.StatusCode} - {response.ErrorMessage}",
                        response.ErrorException);
                }
                var apiresponse = JsonSerializer.Deserialize<AudioApiResponse>(response.Content);
                return Convert.FromBase64String(apiresponse.audio_data);
            }
            catch (Exception ex) {
                _logger.LogError("No audio data received", ex);
                return null;
            }
            // return response.RawBytes ?? throw new InvalidOperationException("No audio data received");
        }


    }
}
