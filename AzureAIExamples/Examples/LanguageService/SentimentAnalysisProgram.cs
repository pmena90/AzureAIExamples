using Azure.AI.TextAnalytics;

namespace AzureAIExamples.Examples.LanguageService
{
    public class SentimentAnalysisProgram : ISentimentAnalysisProgram
    {
        private readonly TextAnalyticsClient _textAnalyticsClient;

        public SentimentAnalysisProgram(TextAnalyticsClient textAnalyticsClient)
        {
            _textAnalyticsClient = textAnalyticsClient;
        }

        public async Task RunAsync()
        {
            Console.WriteLine("Running Sentiment Analysis Example...");
            string document = "I had a wonderful experience! The rooms were clean and the staff was friendly.";

            DocumentSentiment documentSentiment = await _textAnalyticsClient
                .AnalyzeSentimentAsync(document);

            Console.WriteLine($"Document sentiment: {documentSentiment.Sentiment}");
            foreach (var sentence in documentSentiment.Sentences)
            {
                Console.WriteLine($"\tSentence: {sentence.Text}");
                Console.WriteLine($"\tSentiment: {sentence.Sentiment}");
                Console.WriteLine($"\tPositive score: {sentence.ConfidenceScores.Positive:0.00}");
                Console.WriteLine($"\tNeutral score: {sentence.ConfidenceScores.Neutral:0.00}");
                Console.WriteLine($"\tNegative score: {sentence.ConfidenceScores.Negative:0.00}");
            }
        }
    }
}
