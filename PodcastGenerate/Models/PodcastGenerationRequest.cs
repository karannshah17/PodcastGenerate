using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace PodcastGenerate.Models
{
    public record PodcastGenerationRequest
    (
    [property: BindProperty] // Required for model binding
    string BlogUrl,

    string? AudioFilePath = null,
    string? ErrorMessage = null,
    bool ShowAudioPlayer = false
);
}
