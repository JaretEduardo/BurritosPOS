using backend.Domain.Documents;
using MongoDB.Driver;
using backend.Application.Interfaces;

namespace backend.Infrastructure.Repositories
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly IMongoCollection<EmployeeInventory> _collection;

        public InventoryRepository(IMongoDatabase database)
        {
            _collection = database.GetCollection<EmployeeInventory>("employee_inventories");
        }

        public async Task<EmployeeInventory?> GetOpenInventoryAsync(int employeeId)
        {
            var filter = Builders<EmployeeInventory>.Filter.And(
                Builders<EmployeeInventory>.Filter.Eq(x => x.EmployeeId, employeeId),
                Builders<EmployeeInventory>.Filter.Eq(x => x.IsClosed, false)
            );

            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task UpdateInventoryAsync(EmployeeInventory inventory)
        {
            if (string.IsNullOrEmpty(inventory.Id))
            {
                await _collection.InsertOneAsync(inventory);
            }
            else
            {
                var filter = Builders<EmployeeInventory>.Filter.Eq(x => x.Id, inventory.Id);
                await _collection.ReplaceOneAsync(filter, inventory);
            }
        }
    }
}
