using backend.Domain.Documents;
using MongoDB.Driver;

namespace backend.Infrastructure.Persistence.MongoDB
{
    public class BurritosMongoContext
    {
        private readonly IMongoDatabase _database;

        public BurritosMongoContext(IMongoDatabase database)
        {
            _database = database;
        }

        public IMongoCollection<SoldLogs> SoldLogs => _database.GetCollection<SoldLogs>("SoldLogs");

        public IMongoCollection<EmployeeInventory> EmployeeInventories => _database.GetCollection<EmployeeInventory>("EmployeeInventories");

        public IMongoCollection<Ticket> Tickets => _database.GetCollection<Ticket>("Tickets");
    }
}
