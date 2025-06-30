namespace PodcastGenerate.Models.Configurations
{
	public class SpeechifyConfig
    {
        public const string ConfigSectionName = "SpeechifyConfig";
        public string ApiKey { get; set; }
		public string Endpoint { get; set; }
		public string DefaultVoiceId { get; set; }
	}
}
