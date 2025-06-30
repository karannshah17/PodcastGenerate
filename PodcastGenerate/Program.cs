using PodcastGenerate.Interface;
using PodcastGenerate.Models.Configurations;
using PodcastGenerate.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
// Add to your service configuration
builder.Services.Configure<FirecrawlConfig>(
    builder.Configuration.GetSection(FirecrawlConfig.ConfigSectionName));
builder.Services.Configure<AzureOpenAIConfig>(
    builder.Configuration.GetSection(AzureOpenAIConfig.ConfigSectionName));
//builder.Services.AddHttpClient<FirecrawlScraper>();
// Add to your service configuration
builder.Services.Configure<SpeechifyConfig>(
    builder.Configuration.GetSection(SpeechifyConfig.ConfigSectionName));

builder.Services.AddSingleton<ITextToSpeechConverter, SpeechifyTTSConverter>();

builder.Services.AddSingleton<IContentSummarizer, AzureOpenAISummarizer>();
builder.Services.AddSingleton<IContentScraper, FirecrawlScraper>();
builder.Services.AddScoped<IBlogToPodcastService, BlogToPodcastService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
