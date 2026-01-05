using AzureAIExamples.Examples.ContentSafityExample;
using AzureAIExamples.Examples.LanguageService;
using AzureAIExamples.Examples.Speech;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

// Load environment variables from appsetting.json file
var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables()
    .Build();

var services = new Microsoft.Extensions.DependencyInjection.ServiceCollection();

services
    .AddSingleton<IConfiguration>(configuration)
    .ConfigureMyConsoleApp(configuration);

var serviceProvider = services.BuildServiceProvider();

var contentSafetyExample = serviceProvider.GetService<IContentSafetyProgram>();
var languageDetectionExample = serviceProvider.GetService<ILanguageDetectionProgram>();
var sentimentAnalysisExample = serviceProvider.GetService<ISentimentAnalysisProgram>();
var speechTranslationExample = serviceProvider.GetService<ISpeechTranslationProgram>();

//if (contentSafetyExample != null)
//    await contentSafetyExample.RunAsync();

//if (languageDetectionExample != null)
//    await languageDetectionExample.RunAsync();

//if (sentimentAnalysisExample != null)
//    await sentimentAnalysisExample.RunAsync();

if (speechTranslationExample != null)
    await speechTranslationExample.RunAsync();
