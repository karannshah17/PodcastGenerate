namespace PodcastGenerate.Interface
{
    public interface ITextToSpeechConverter
    {
        Task<byte[]> ConvertToSpeechAsync(string text);
    }
}
