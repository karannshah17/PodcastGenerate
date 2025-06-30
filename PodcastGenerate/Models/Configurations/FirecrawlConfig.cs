namespace PodcastGenerate.Models.Configurations
{
	public class FirecrawlConfig
	{
        public const string ConfigSectionName = "FirecrawlConfig";
        public string ApiKey { get; set; }
		public string Endpoint { get; set; }
	}
}
