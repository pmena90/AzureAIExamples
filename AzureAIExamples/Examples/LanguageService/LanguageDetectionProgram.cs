using Azure.AI.TextAnalytics;

namespace AzureAIExamples.Examples.LanguageService
{
    public class LanguageDetectionProgram : IExampleProgram
    {
        private readonly TextAnalyticsClient _textAnalyticsClient;

        public LanguageDetectionProgram(TextAnalyticsClient textAnalyticsClient)
        {
            _textAnalyticsClient = textAnalyticsClient;
        }

        public async Task RunAsync()
        {
            var inputText = "Bonjour, comment allez-vous?";
            var response = await _textAnalyticsClient.DetectLanguageAsync(inputText);

            Console.WriteLine($"Input: {inputText}");
            Console.WriteLine($"Detected language: {response.Value.Name} (ISO6391: {response.Value.Iso6391Name})");
        }
    }
}
