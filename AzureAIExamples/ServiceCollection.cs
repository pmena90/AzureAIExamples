using Azure;
using Azure.AI.ContentSafety;
using Azure.AI.TextAnalytics;
using AzureAIExamples.Examples;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class ServiceCollection
{
    public static IServiceCollection ConfigureMyConsoleApp(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddContentSafetyClients(configuration);
        services.AddProgramExamples();
        services.AddLanguageServiceClient(configuration);

        return services;
    }

    private static IServiceCollection AddContentSafetyClients(this IServiceCollection services, IConfiguration configuration)
    {
        var endpoint = configuration["AzureContentSafety:Endpoint"];
        var key = configuration["AzureContentSafety:ApiKey"];

        if (string.IsNullOrWhiteSpace(endpoint) || string.IsNullOrWhiteSpace(key))
        {
            throw new InvalidOperationException("Missing Azure Content Safety configuration.");
        }

        services.AddSingleton(new ContentSafetyClient(
            new Uri(endpoint),
            new AzureKeyCredential(key)));

        services.AddSingleton(new BlocklistClient(
            new Uri(endpoint),
            new AzureKeyCredential(key)));

        return services;
    }

    private static IServiceCollection AddProgramExamples(this IServiceCollection services)
    {
        // lets use scrutor to add all the IProgram implementations automatically
        services.Scan(scan => scan
            .FromAssembliesOf(typeof(IExampleProgram))
            .AddClasses(classes => classes.AssignableTo<IExampleProgram>())
            .AsImplementedInterfaces()
            .WithSingletonLifetime());
        return services;
    }

    private static IServiceCollection AddLanguageServiceClient(this IServiceCollection services, IConfiguration configuration)
    {
        var endpoint = configuration["AzureLanguageService:Endpoint"];
        var key = configuration["AzureLanguageService:ApiKey"];

        if (string.IsNullOrWhiteSpace(endpoint) || string.IsNullOrWhiteSpace(key))
        {
            throw new InvalidOperationException("Missing Azure Language Service configuration.");
        }

        services.AddSingleton(new TextAnalyticsClient(
            new Uri(endpoint),
            new AzureKeyCredential(key)));

        return services;
    }
}
