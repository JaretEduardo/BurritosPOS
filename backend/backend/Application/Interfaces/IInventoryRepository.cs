using backend.Domain.Documents;

namespace backend.Application.Interfaces
{
    public interface IInventoryRepository
    {
        Task<EmployeeInventory?> GetOpenInventoryAsync(int employeeId);

        Task UpdateInventoryAsync(EmployeeInventory inventory);
    }
}
