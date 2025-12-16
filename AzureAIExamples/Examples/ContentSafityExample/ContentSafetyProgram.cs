using Azure.AI.ContentSafety;
using Azure.Core;
using AzureAIExamples.Examples.ContentSafityExample;

namespace AzureAIExamples.Examples.ContentSafetyExample;

public class ContentSafetyProgram : IContentSafetyProgram
{
    private readonly ContentSafetyClient _contentSafetyClient;
    private readonly BlocklistClient _blocklistClient;

    private const string BlocklistName = "TestBlocklist";
    private const string BlocklistDescription = "Test blocklist management";

    public ContentSafetyProgram(ContentSafetyClient contentSafetyClient, BlocklistClient blocklistClient)
    {
        _contentSafetyClient = contentSafetyClient;
        _blocklistClient = blocklistClient;
    }

    public async Task RunAsync()
    {
        // --------------------
        // BLOCKLIST SETUP
        // --------------------
        //AddBlocklistItems();

        // --------------------
        // TEXT MODERATION
        // --------------------
        //await TextModerationExampleWithBlocklistAsync();

        // --------------------
        // IMAGE MODERATION
        // --------------------
        await ImageModerationExampleAsync();
    }

    private void AddBlocklistItems()
    {
        var data = new
        {
            description = BlocklistDescription,
        };

        var createResponse = _blocklistClient.CreateOrUpdateTextBlocklist(BlocklistName, RequestContent.Create(data));
        if (createResponse.Status == 201)
        {
            Console.WriteLine("\nBlocklist {0} created.", BlocklistName);
        }
        else if (createResponse.Status == 200)
        {
            Console.WriteLine("\nBlocklist {0} updated.", BlocklistName);
        }

        string blockItemText1 = "kill";
        string blockItemText2 = "hate";

        var blockItems = new TextBlocklistItem[] { new TextBlocklistItem(blockItemText1), new TextBlocklistItem(blockItemText2) };
        var addedBlockItems = _blocklistClient.AddOrUpdateBlocklistItems(BlocklistName, new AddOrUpdateTextBlocklistItemsOptions(blockItems));

        if (addedBlockItems != null && addedBlockItems.Value != null)
        {
            Console.WriteLine("\nBlockItems added:");
            foreach (var addedBlockItem in addedBlockItems.Value.BlocklistItems)
            {
                Console.WriteLine("BlockItemId: {0}, Text: {1}, Description: {2}", addedBlockItem.BlocklistItemId, addedBlockItem.Text, addedBlockItem.Description);
            }
        }
    }

    private async Task TextModerationExampleWithBlocklistAsync()
    {
        var textRequest = new AnalyzeTextOptions("I hate you and want to hurt you")
        {
            Categories = { TextCategory.Hate, TextCategory.Violence, TextCategory.SelfHarm, TextCategory.Violence },
            BlocklistNames = { BlocklistName },
        };

        var textResponse = await _contentSafetyClient.AnalyzeTextAsync(textRequest);

        Console.WriteLine("Text moderation result:");
        foreach (var category in textResponse.Value.CategoriesAnalysis)
        {
            Console.WriteLine($"- {category.Category}: Severity {category.Severity}");
        }

        Console.WriteLine("Matched Blocklist Items:");
        foreach (var match in textResponse.Value.BlocklistsMatch)
        {
            Console.WriteLine($"- Blocklist Name: {match.BlocklistName} -> Item Text: {match.BlocklistItemText}");
        }
    }

    private async Task ImageModerationExampleAsync()
    {
        // Image example with potential sexual content
        var imageUrl = new Uri("https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTvxyX0DyUsMc0ALV0iOirutLj70PE0-e0_nw&s");
        var imageContent = BinaryData.FromStream(await new HttpClient().GetStreamAsync(imageUrl));

        var imageData = new ContentSafetyImageData(imageContent);

        var imageRequest = new AnalyzeImageOptions(imageData);

        var imageResponse = await _contentSafetyClient.AnalyzeImageAsync(imageRequest);

        Console.WriteLine("\nImage moderation result:");
        foreach (var category in imageResponse.Value.CategoriesAnalysis)
        {
            Console.WriteLine($"- {category.Category}: Severity {category.Severity}");
        }
    }
}
