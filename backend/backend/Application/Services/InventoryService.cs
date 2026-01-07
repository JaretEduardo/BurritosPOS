using backend.Application.DTOs;
using backend.Application.Interfaces;
using backend.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using backend.Domain.Documents;

namespace backend.Application.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly IProductRepository _productRepository;
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IConfiguration _configuration;

        public InventoryService(IProductRepository productRepository, IInventoryRepository inventoryRepository, IConfiguration configuration)
        {
            _productRepository = productRepository;
            _inventoryRepository = inventoryRepository;
            _configuration = configuration;
        }

        public async Task<Product> ProductAsync(ProductDto dto)
        {
            var product = new Product
            {
                Id = dto.Id,
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price
            };

            await _productRepository.AddAsync(product);

            return product;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _productRepository.GetAllAsync();
        }

        public async Task RegisterInventoryAsync(EmployeeInventoryDto dto)
        {
            var currentInventory = await _inventoryRepository.GetOpenInventoryAsync(dto.EmployeeId);

            if (currentInventory != null)
            {
                currentInventory.Products = dto.Products.Select(p => new InventoryProduct
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    Quantity = p.Quantity
                }).ToList();

                await _inventoryRepository.UpdateInventoryAsync(currentInventory);
            }
            else
            {
                var newInventory = new EmployeeInventory
                {
                    EmployeeId = dto.EmployeeId,
                    InventoryDate = DateTime.UtcNow,
                    CreatedAt = DateTime.UtcNow,
                    IsClosed = false,

                    Products = dto.Products.Select(p => new InventoryProduct
                    {
                        ProductId = p.ProductId,
                        ProductName = p.ProductName,
                        Quantity = p.Quantity
                    }).ToList()
                };

                await _inventoryRepository.UpdateInventoryAsync(newInventory);
            }
        }

        public async Task CloseShiftAsync(int employeeId)
        {
            var openInventory = await _inventoryRepository.GetOpenInventoryAsync(employeeId);

            if (openInventory == null)
            {
                throw new Exception("No tienes un turno abierto para cerrar.");
            }

            openInventory.IsClosed = true;
            openInventory.ClosedAt = DateTime.UtcNow;

            await _inventoryRepository.UpdateInventoryAsync(openInventory);
        }
    }
}
