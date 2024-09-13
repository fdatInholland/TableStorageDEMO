
using Azure;
using Azure.Data.Tables;
using Azure.Storage.Blobs.Models;
using Domain;

namespace ConsoleApp1
{
    internal class Program
    {
        private const string TableName = "Product";
        private static readonly string ConnectionString = "UseDevelopmentStorage=true";

        static async Task Main(string[] args)
        {
            // Create a new TableClient instance
            var serviceClient = new TableServiceClient(ConnectionString);
            var tableClient = serviceClient.GetTableClient(TableName);

            // Create a table if it doesn't exist
            await tableClient.CreateIfNotExistsAsync();

            //Insert new product
            Product newproduct = new Product("Electronics", "12345", "Laptop", 999.99m);
            await tableClient.AddEntityAsync(newproduct).ConfigureAwait(false);

            // Retrieve the entity

            var retrievedProduct = await tableClient.GetEntityAsync<Product>("Electronics", "12345");
            Console.WriteLine($"Retrieved product: {retrievedProduct.Value.Name}, Price: {retrievedProduct.Value.Price.ToString()}");


            // Update the entity
            Product updateProduct = new Product("Electronics", "12345", "Laptop", 899.99m);
            await tableClient.UpdateEntityAsync(updateProduct, ETag.All, TableUpdateMode.Replace);

            //filtering
            var filter = TableClient.CreateQueryFilter($"Price gt {400}");

            // Execute the query
            var entities = tableClient.QueryAsync<Product>(filter);

            var result = new List<Product>();
            await foreach (var entity in entities)
            {
                result.Add(entity);
            }
        }
    }
}
