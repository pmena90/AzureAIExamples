using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using Microsoft.CognitiveServices.Speech.Translation;
using Microsoft.Extensions.Configuration;

namespace AzureAIExamples.Examples.Speech
{
    public class SpeechTranslationProgram : ISpeechTranslationProgram
    {
        private readonly IConfiguration _configuration;

        public SpeechTranslationProgram(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task RunAsync()
        {
            var region = _configuration["AzureSpeech:Region"];
            var key = _configuration["AzureSpeech:Key"];

            var config = SpeechTranslationConfig.FromSubscription(key, region);
            config.SpeechRecognitionLanguage = "es-ES";
            config.AddTargetLanguage("en");

            using var audioConfig = AudioConfig.FromDefaultMicrophoneInput();
            using var recognizer = new TranslationRecognizer(config, audioConfig);

            Console.WriteLine("Say something in Spanish...");

            var result = await recognizer.RecognizeOnceAsync();

            if (result.Reason == ResultReason.TranslatedSpeech)
            {
                var translation = result.Translations["en"];
                Console.WriteLine($"Recognized (es): {result.Text}");
                Console.WriteLine($"Translated (en): {translation}");

                // Speak the translation in English
                var speechConfig = SpeechConfig.FromSubscription(key, region);
                speechConfig.SpeechSynthesisLanguage = "en-US";
                using var synthesizer = new SpeechSynthesizer(speechConfig);
                await synthesizer.SpeakTextAsync(translation);
            }
            else
            {
                Console.WriteLine($"Speech Recognition failed: {result.Reason}");
            }
        }
    }
}
