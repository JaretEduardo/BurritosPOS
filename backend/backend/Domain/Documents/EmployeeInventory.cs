using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace backend.Domain.Documents
{
    public class EmployeeInventory
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("employee_id")]
        public int EmployeeId { get; set; }

        [BsonElement("products")]
        public List<InventoryProduct> Products { get; set; } = new List<InventoryProduct>();

        [BsonElement("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public class InventoryProduct
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
    }
}
