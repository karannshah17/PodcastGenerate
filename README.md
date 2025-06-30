# PodcastGenerate

PodcastGenerate is a web application built on .NET 8 to automate the creation of podcasts from web content. It leverages modern AI tools for content scraping, summarization, and text-to-speech audio generation.

## Features

- **Content Scraping:** Uses Firecrawl to extract high-quality, main-content text from any URL, including blogs and articles.
- **AI Summarization:** Integrates Azure OpenAI for summarizing scraped text into a podcast-ready script.
- **Text-to-Speech:** Converts scripts into realistic speech audio with Speechify.
- **Modular Service Architecture:** Easily extendable with new AI or content services.

## Technology Stack

- **.NET 8 (ASP.NET Core)**
- **Azure.AI.OpenAI** for summarization
- **Firecrawl** for web scraping
- **Speechify** for text-to-speech
- **RestSharp** for HTTP requests

## Integrations

### Firecrawl

Firecrawl is used to scrape and extract the primary content from any webpage.  
- **How it works:** The `FirecrawlScraper` service sends a request to the Firecrawl API, retrieves the most relevant content in markdown format, and prepares it for summarization.
- **Configuration:**
  ```json
  "FirecrawlConfig": {
    "ApiKey": "<FIRECRAWL_API_KEY>",
    "Endpoint": "https://api.firecrawl.dev/v1/scrape"
  }
  ```
- **Code Reference:** See [`FirecrawlScraper.cs`](PodcastGenerate/Services/FirecrawlScraper.cs)

### Speechify

Speechify provides neural text-to-speech conversion for generating podcast audio.
- **How it works:** The `SpeechifyTTSConverter` service sends the podcast script to Speechify's API and returns the resulting audio data.
- **Configuration:**
  ```json
  "SpeechifyConfig": {
    "ApiKey": "<SPEECHIFY_API_KEY>",
    "Endpoint": "https://api.sws.speechify.com/v1/audio/speech",
    "DefaultVoiceId": "<VOICE_ID>"
  }
  ```
- **Code Reference:** See [`SpeechifyTTSConverter.cs`](PodcastGenerate/Services/SpeechifyTTSConverter.cs)

## Project Structure

```
PodcastGenerate/
├── PodcastGenerate.csproj
├── Pages/
│   └── Services/
│       ├── FirecrawlScraper.cs
│       ├── SpeechifyTTSConverter.cs
│       └── ...
├── Models/
│   └── Configurations/
│       ├── FirecrawlConfig.cs
│       ├── SpeechifyConfig.cs
│       └── ...
├── appsettings.json
└── Program.cs
```

## Getting Started

1. **Clone the repository:**
   ```bash
   git clone https://github.com/karannshah17/PodcastGenerate.git
   ```
2. **Navigate to the project directory:**
   ```bash
   cd PodcastGenerate
   ```
3. **Set your API keys** in `appsettings.json` for AzureOpenAI, Firecrawl, and Speechify.
4. **Restore dependencies:**
   ```bash
   dotnet restore
   ```
5. **Run the application:**
   ```bash
   dotnet run --project PodcastGenerate
   ```

## Configuration

Edit `appsettings.json` to add your API keys and desired configuration for:
- Azure OpenAI
- Firecrawl
- Speechify

You can also customize voice IDs, output directories, and other settings.

## License

This project does not currently specify a license.

---

For more details, see the [GitHub repository](https://github.com/karannshah17/PodcastGenerate).
