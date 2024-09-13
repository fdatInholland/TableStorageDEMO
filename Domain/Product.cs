using Azure;
using Azure.Data.Tables;
using Microsoft.WindowsAzure.Storage;

namespace Domain
{
    public class Product : ITableEntity
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }

        public string Name { get; set; }
        public decimal Price { get; set; }

        //required for deserialisation
        public Product()
        {

        }

        public Product(string partitionKey, string rowKey, string name, decimal price)
        {
            PartitionKey = partitionKey;
            RowKey = rowKey;
            Name = name;
            Price = price;
        }

        // Required for ITableEntity
        public void ReadEntity(IDictionary<string, object> properties, OperationContext operationContext)
        {
            PartitionKey = properties[nameof(PartitionKey)].ToString();
            RowKey = properties[nameof(RowKey)].ToString();
            Timestamp = (DateTimeOffset?)properties[nameof(Timestamp)];
            ETag = (ETag)properties[nameof(ETag)];

            Name = properties[nameof(Name)].ToString();
            Price = Convert.ToDecimal(properties[nameof(Price)]);
        }

        public IDictionary<string, object> WriteEntity(OperationContext operationContext)
        {
            var properties = new Dictionary<string, object>
        {
            { nameof(PartitionKey), PartitionKey },
            { nameof(RowKey), RowKey },
            { nameof(Timestamp), Timestamp },
            { nameof(ETag), ETag },
            { nameof(Name), Name },
            { nameof(Price), Price }
        };

            return properties;
        }
    }
}
