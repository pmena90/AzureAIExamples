using AzureAIExamples.Examples.ContentSafityExample;
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

var contentSafetyExample = services
    .BuildServiceProvider()
    .GetService<IContentSafetyProgram>();

if (contentSafetyExample != null)
    await contentSafetyExample.RunAsync();

var languageDetectionExample = services
    .BuildServiceProvider()
    .GetService<ILanguageDetectionProgram>();

if (languageDetectionExample != null)
    await languageDetectionExample.RunAsync();
