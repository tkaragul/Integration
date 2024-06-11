# MyProject

## Description

MyProject is a service designed to handle the integration of items from a third party. The project is divided into two layers: Service and Backend. The goal is to ensure that each item content is processed and saved only once, even when multiple concurrent requests are made. 

## Project Structure

MyProject
Service
ItemIntegrationService.cs
Backend
ItemRepository.cs
Program.cs
DistributedSolutionWeaknesses.txt


## Service Layer

### ItemIntegrationService.cs

The `ItemIntegrationService` class is responsible for integrating items and managing the processing to ensure that no duplicate items are processed concurrently.

```csharp
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using MyProject.Backend;

namespace MyProject.Service
{
    public class ItemIntegrationService
    {
        private readonly ItemRepository _repository;
        private static readonly ConcurrentDictionary<string, string> ProcessingItems = new ConcurrentDictionary<string, string>();

        public ItemIntegrationService(ItemRepository repository)
        {
            _repository = repository;
        }

        public async Task<string> IntegrateItemAsync(string content)
        {
            if (ProcessingItems.ContainsKey(content))
            {
                return "Duplicate item";
            }

            if (ProcessingItems.TryAdd(content, content))
            {
                try
                {
                    await Task.Delay(2000); // Simulate processing time

                    var itemId = await _repository.SaveItemAsync(content);
                    return itemId.ToString();
                }
                finally
                {
                    ProcessingItems.TryRemove(content, out _);
                }
            }
            else
            {
                return "Duplicate item";
            }
        }
    }
}
