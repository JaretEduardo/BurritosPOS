using backend.Domain.Entities;

namespace backend.Application.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<Employee?> GetByEmailAsync(string email);

        Task<Employee?> GetByIdAsync(int id);

        Task AddAsync(Employee employee);

        Task UpdateAsync(Employee employee);
    }
}
