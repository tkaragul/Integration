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

```

## Backend Layer
### ItemRepository.cs
The ItemRepository class simulates the storage of items in a database and ensures that no duplicate items are saved.

```csharp
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyProject.Backend
{
    public class ItemRepository
    {
        private readonly HashSet<string> _items = new HashSet<string>();
        private int _currentId = 0;

        public async Task<int> SaveItemAsync(string content)
        {
            await Task.Delay(100); // Simulate database save delay

            if (_items.Contains(content))
            {
                throw new InvalidOperationException("Item already exists");
            }

            _items.Add(content);
            return ++_currentId;
        }
    }
}

```
## Main Program
### Program.cs
The Program.cs file demonstrates the parallel processing and storage of items with different content.

```csharp
using System;
using System.Linq;
using System.Threading.Tasks;
using MyProject.Service;
using MyProject.Backend;

namespace MyProject
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var repository = new ItemRepository();
            var service = new ItemIntegrationService(repository);

            var tasks = Enumerable.Range(0, 10).Select(i => service.IntegrateItemAsync($"ItemContent{i % 3}")).ToArray();
            var results = await Task.WhenAll(tasks);

            foreach (var result in results)
            {
                Console.WriteLine(result);
            }
        }
    }
}

```
## Distributed System Scenario
In a distributed scenario where multiple servers are running the ItemIntegrationService, a centralized coordination service such as Redis would be necessary to prevent duplicate processing of items.

## DistributedSolutionWeaknesses.txt
This file outlines the potential weaknesses of using a centralized coordination service like Redis:

1. Single Point of Failure: Using a centralized coordination service introduces a single point of failure. If Redis becomes unavailable, the entire item processing mechanism could be disrupted.

2. Latency: Network latency can increase the time taken to acquire and release locks, potentially slowing down the processing time.

3. Complexity: Implementing distributed locks adds complexity to the system, requiring additional components and configurations. This added complexity can make the system harder to maintain and debug.


## How to Run
Clone the repository:
git clone https://github.com/tkaragul/Integration.git
cd MyProject

## Build the project:
dotnet build

## Run the project:
dotnet run

## Conclusion
This project demonstrates how to handle concurrent item processing and ensure that each item is processed and saved only once, even when multiple concurrent requests are made. The solution uses ConcurrentDictionary to manage the processing state and HashSet in the repository to ensure unique storage.


Bu README dosyası, projenizin amacını, içeriğini ve nasıl çalıştırılacağını detaylı bir şekilde açıklar. Bu, projeyi kullanmak veya geliştirmek isteyen kişiler için yararlı olacaktır.



