using backend.Application.DTOs;
using backend.Domain.Entities;

namespace backend.Application.Interfaces
{
    public interface IInventoryService
    {
        Task<Product> ProductAsync(ProductDto dto);

        Task<IEnumerable<Product>> GetAllProductsAsync();

        Task RegisterInventoryAsync(EmployeeInventoryDto dto);

        Task CloseShiftAsync(int employeeId);

        Task<EmployeeInventoryDto?> GetOpenInventoryByEmployeeAsync(int employeeId);
    }
}
