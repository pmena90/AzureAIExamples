using Azure;
using Azure.AI.ContentSafety;
using AzureAIExamples.Examples;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class ServiceCollection
{
    public static IServiceCollection AddContentSafetyClients(this IServiceCollection services, IConfiguration configuration)
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

    public static IServiceCollection AddProgramExamples(this IServiceCollection services)
    {
        // lets use scrutor to add all the IProgram implementations automatically
        services.Scan(scan => scan
            .FromAssembliesOf(typeof(IExampleProgram))
            .AddClasses(classes => classes.AssignableTo<IExampleProgram>())
            .AsImplementedInterfaces()
            .WithSingletonLifetime());
        return services;
    }
}
