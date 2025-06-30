using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OpenAI.Chat;
using PodcastGenerate.Interface;
using PodcastGenerate.Models;
using PodcastGenerate.Models.Configurations;

namespace PodcastGenerate.Pages
{
    public class GeneratePodcastModel : PageModel
	{
		
		//private readonly ApplicationSettings _appSettings;
		private readonly ILogger<GeneratePodcastModel> _logger;
        private readonly IBlogToPodcastService _podcastService;

        [BindProperty]
        public PodcastGenerationRequest PodcastRequest { get;  set; } = new(BlogUrl: "");
        public byte[]? AudioBytes { get; private set; }  // Add this property

        public GeneratePodcastModel(
			
			//IOptions<ApplicationSettings> appSettings,
			ILogger<GeneratePodcastModel> logger, IBlogToPodcastService podcastService)
		{
			
            //_appSettings = appSettings.Value;
            _podcastService = podcastService;
            _logger = logger;
		}

		public void OnGet()
		{
		}

		public async Task<IActionResult> OnPostAsync()
		{
				if (string.IsNullOrEmpty(PodcastRequest.BlogUrl))
				{
					PodcastRequest = PodcastRequest with
					{
						ErrorMessage = "Please enter a blog URL first."
					};
					return Page();
				}
				try
				{
					AudioBytes = await _podcastService.ConvertBlogToPodcastAsync(PodcastRequest.BlogUrl);
					if (AudioBytes == null || AudioBytes.Length == 0)
					{
						PodcastRequest = PodcastRequest with
						{
						    ErrorMessage = "Failed to generate podcast audio."
						};
							 return Page();
                    
                    
					}
					
					PodcastRequest = PodcastRequest with { ShowAudioPlayer = true };

				}
				catch (Exception ex)
				{
                _logger.LogError(ex, "Error generating podcast for {BlogUrl}", PodcastRequest.BlogUrl);
                PodcastRequest = PodcastRequest with
                {
                    ErrorMessage = "An error occurred while generating the podcast. Please try again"
                };
               
                 
				}
            return Page();


        }
	}
}		

