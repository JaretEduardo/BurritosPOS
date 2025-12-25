using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace backend.Domain.Documents
{
    public class Ticket
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("items")]
        public List<TicketItem> Items { get; set; } = new List<TicketItem>();

        [BsonElement("total")]
        public decimal Total { get; set; }

        [BsonElement("sold_by")]
        public TicketEmployee SoldBy { get; set; }
    }

    public class TicketItem
    {
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }

    public class TicketEmployee
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
    }
}
