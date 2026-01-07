using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace backend.Domain.Documents
{
    public class EmployeeInventory
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonIgnoreIfDefault]
        public string Id { get; set; }

        [BsonElement("employee_id")]
        public int EmployeeId { get; set; }

        [BsonElement("inventory_date")]
        public DateTime InventoryDate { get; set; }

        [BsonElement("created_at")]
        public DateTime CreatedAt { get; set; }

        [BsonElement("is_closed")]
        public bool IsClosed { get; set; } = false;

        [BsonElement("closed_at")]
        public DateTime? ClosedAt { get; set; }

        [BsonElement("products")]
        public List<InventoryProduct> Products { get; set; } = new List<InventoryProduct>();
    }

    public class InventoryProduct
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
    }
}
