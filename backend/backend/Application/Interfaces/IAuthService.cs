using backend.Application.DTOs;
using backend.Domain.Entities;

namespace backend.Application.Interfaces
{
    public interface IAuthService
    {
        Task<Employee> RegisterAsync(RegisterDto dto);

        Task<string> LoginAsync(LoginDto dto);
    }
}
