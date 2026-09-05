
using Azure;
using Azure.Data.Tables;
using Domain;

namespace ConsoleApp1
{
    internal class Program
    {
        private const string TableName = "Product";
        private static readonly string ConnectionString = "UseDevelopmentStorage=true";

        static async Task Main(string[] args)
        {
            // Create a new TableServiceClient instance
            var serviceClient = new TableServiceClient(ConnectionString);
            var tableClient = serviceClient.GetTableClient(TableName);

            // Create a table if it doesn't exist
            try
            {
                //include retry policy to handle transient failures
                await tableClient.CreateIfNotExistsAsync();
            }
            catch (RequestFailedException ex) 
            {

            }


            //// 1. Insert new product (Price stored as double)
            Product newProduct = new Product("Electronics", "12345", "Laptop", 999.99);
            await tableClient.AddEntityAsync(newProduct).ConfigureAwait(false);

            //// 2. Retrieve the entity
            Product retrievedProduct = await tableClient.GetEntityAsync<Product>("Electronics", "12345");
            Console.WriteLine($"Retrieved product: {retrievedProduct.Name}, Price: {retrievedProduct.Price}");

            // 3. Update the entity
            try
            {
                Product updateProduct = new Product("Electronics", "12345", "Laptop", 899.99);

                Response response = await tableClient.UpsertEntityAsync(updateProduct, TableUpdateMode.Replace);
                Console.WriteLine($"Update succeeded with HTTP Status: {response.Status}");
            }
            catch (RequestFailedException ex)
            {
                Console.WriteLine($"Update failed: {ex.Status} - {ex.ErrorCode}");
                Console.WriteLine(ex.Message);
            }

            // 4. Filtering (Pass double value 400.0d to match double schema)
            double minPrice = 400.0;
            string filter = TableClient.CreateQueryFilter($"Price gt {minPrice}");

            // 5. Execute the query asynchronously
            AsyncPageable<Product> entities = tableClient.QueryAsync<Product>(filter);

            List<Product> Productresults = new List<Product>();
            await foreach (var entity in entities)
            {
                Productresults.Add(entity);
            }

            Console.WriteLine($"Found {Productresults.Count} products matching the filter.");
        }
    }
}

