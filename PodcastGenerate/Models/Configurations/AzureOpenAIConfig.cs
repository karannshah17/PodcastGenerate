namespace PodcastGenerate.Models.Configurations
{
	public class AzureOpenAIConfig
	{
        public const string ConfigSectionName = "AzureOpenAIConfig";
        public string ApiKey { get; set; }
		public string Endpoint { get; set; }
		public string Model { get; set; }
		public string Prompt { get; set; }

    }
}
