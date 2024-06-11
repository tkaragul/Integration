using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Integration.Backend;
public class ItemRepository
{
    /// <summary>
    /// The reason I use HashSet<string> is that it provides a fast and efficient way to check if elements are unique. 
    /// HashSet collection has O(1) time complexity to check if an element is present, which is advantageous in terms of performance.
    /// </summary>
    private readonly HashSet<string> _items = new HashSet<string>();
    private int _currentId = 0;

    /// <summary>
    /// Save item to database
    /// </summary>
    /// <param name="content">Represents item content.</param>
    /// <returns>Returns saved currentId</returns>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task<int> SaveItemAsync(string content)
    {
        await Task.Delay(100); // Simulate database save delay

        // Throw error if content already exists
        if (_items.Contains(content))
        {
            throw new InvalidOperationException("Item already exists");
        }

        // Add new item and increase ID
        _items.Add(content);
        return ++_currentId;
    }
}
