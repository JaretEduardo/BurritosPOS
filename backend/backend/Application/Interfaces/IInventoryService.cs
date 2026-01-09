using backend.Application.DTOs;
using backend.Domain.Entities;

namespace backend.Application.Interfaces
{
    public interface IInventoryService
    {
        Task<ServiceResponseDto<Product>> ProductAsync(ProductDto dto);

        Task<ServiceResponseDto<IEnumerable<Product>>> GetAllProductsAsync();

        Task<ServiceResponseDto<bool>> RegisterInventoryAsync(EmployeeInventoryDto dto);

        Task<ServiceResponseDto<bool>> CloseShiftAsync(int employeeId);

        Task<ServiceResponseDto<EmployeeInventoryDto?>> GetOpenInventoryByEmployeeAsync(int employeeId);
    }
}
