using Azure;
using Azure.Data.Tables;
using System.Diagnostics;

namespace Domain
{
    public class Product : ITableEntity
    {
        public string PartitionKey { get; set; } = string.Empty;
        public string RowKey { get; set; } = string.Empty;
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }

        public string Name { get; set; } = string.Empty;
        public double Price { get; set; }
        
        //required for deserialisation
        public Product()
        {

        }

        public Product(string partitionKey, string rowKey, string name, double price)
        {
            PartitionKey = partitionKey;
            RowKey = rowKey;
            Name = name;
            Price = price;
        }

    }
}
