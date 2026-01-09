using backend.Application.DTOs;
using backend.Application.Interfaces;
using backend.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace backend.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IEmployeeRepository _repository;
        private readonly IConfiguration _configuration;

        public AuthService(IEmployeeRepository repository, IConfiguration configuration)
        {
            _repository = repository;
            _configuration = configuration;
        }

        public async Task<ServiceResponseDto<Employee>> RegisterAsync(RegisterDto dto)
        {
            var existingUser = await _repository.GetByEmailAsync(dto.Email);

            var response = new ServiceResponseDto<Employee>();

            if (existingUser != null)
            {
                response.Status = false;
                response.Message = "El email ya está registrado.";
                return response;
            }

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            var newEmployee = new Employee
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber
            };

            await _repository.AddAsync(newEmployee);

            response.Status = true;
            response.Message = "Usuario registrado exitosamente";
            response.Data = newEmployee;

            return response;
        }

        public async Task<ServiceResponseDto<string>> LoginAsync(LoginDto dto)
        {
            var user = await _repository.GetByEmailAsync(dto.Email);

            var response = new ServiceResponseDto<string>();

            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            {
                response.Status = false;
                response.Message = "Credenciales inválidas (Usuario o contraseña incorrectos).";
                response.Data = null;
                return response;
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.FirstName)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("Jwt:Key").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1), // El token dura 1 día
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenConfig = tokenHandler.CreateToken(tokenDescriptor);

            string tokenFinal = tokenHandler.WriteToken(tokenConfig);

            response.Data = tokenFinal;
            response.Status = true;
            response.Message = "Login exitoso";

            return response;
        }
    }
}
