using Integration.Backend;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Integration.Service;
public class ItemIntegrationService
{
    private readonly ItemRepository _repository;

    /// <summary>
    /// The reason I use ConcurrentDictionary<string, string> is to provide a safe data structure
    /// so that multiple threads can process elements simultaneously and prevent the same element from being processed multiple times.
    /// </summary>
    private static readonly ConcurrentDictionary<string, string> ProcessingItems = new ConcurrentDictionary<string, string>();

    public ItemIntegrationService(ItemRepository repository)
    {
        _repository = repository;
    }

    // Method that integrates and saves the element
    public async Task<string> IntegrateItemAsync(string content)
    {
        // If the content is already processed, "Duplicate item" is returned
        if (ProcessingItems.ContainsKey(content))
        {
            return "Duplicate item";
        }

        // Content is marked as processing
        if (ProcessingItems.TryAdd(content, content))
        {
            try
            {
                // Simulate processing time
                await Task.Delay(2000);

                // Save item
                var itemId = await _repository.SaveItemAsync(content);
                return itemId.ToString();
            }
            finally
            {
                // Removes the content from the dictionary when the operation is completed
                ProcessingItems.TryRemove(content, out _);
            }
        }
        else
        {
            return "Duplicate item";
        }
    }
}
