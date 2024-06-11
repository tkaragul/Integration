// See https://aka.ms/new-console-template for more information
using Integration.Service;
using Integration.Backend;

var repository = new ItemRepository();
var service = new ItemIntegrationService(repository);

// Creates tasks to process items with the same content in parallel
var tasks = Enumerable.Range(0, 10).Select(i => service.IntegrateItemAsync($"ItemContent{i % 3}")).ToArray();
var results = await Task.WhenAll(tasks);

// print results
foreach (var result in results)
{
    Console.WriteLine(result);
}

