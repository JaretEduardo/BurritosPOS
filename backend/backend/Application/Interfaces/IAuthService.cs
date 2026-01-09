using backend.Application.DTOs;
using backend.Domain.Entities;

namespace backend.Application.Interfaces
{
    public interface IAuthService
    {
        Task<ServiceResponseDto<Employee>> RegisterAsync(RegisterDto dto);

        Task<ServiceResponseDto<string>> LoginAsync(LoginDto dto);
    }
}
