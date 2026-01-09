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

        public async Task<ServiceResponseDto<Product>> ProductAsync(ProductDto dto)
        {
            var response = new ServiceResponseDto<Product>();

            try
            {
                var product = new Product
                {
                    Id = dto.Id,
                    Name = dto.Name,
                    Description = dto.Description,
                    Price = dto.Price
                };

                await _productRepository.AddAsync(product);

                response.Data = product;
                response.Status = true;
                response.Message = "Producto registrado correctamente";
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = "Error al registrar producto: " + ex.Message;
            }

            return response;
        }

        public async Task<ServiceResponseDto<IEnumerable<Product>>> GetAllProductsAsync()
        {
            var response = new ServiceResponseDto<IEnumerable<Product>>();

            try
            {
                var products = await _productRepository.GetAllAsync();

                response.Data = products;
                response.Status = true;
                response.Message = "Productos obtenidos exitosamente";
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = "Error al obtener productos: " + ex.Message;
            }

            return response;
        }

        public async Task<ServiceResponseDto<bool>> RegisterInventoryAsync(EmployeeInventoryDto dto)
        {
            var response = new ServiceResponseDto<bool>();

            try
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

                    response.Message = "Inventario actualizado correctamente";
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

                    response.Message = "Nuevo turno de inventario iniciado";
                }

                response.Data = true;
                response.Status = true;
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = "Error al guardar inventario: " + ex.Message;
                response.Data = false;
            }

            return response;
        }

        public async Task<ServiceResponseDto<bool>> CloseShiftAsync(int employeeId)
        {
            var response = new ServiceResponseDto<bool>();

            try
            {
                var openInventory = await _inventoryRepository.GetOpenInventoryAsync(employeeId);

                if (openInventory == null)
                {
                    response.Status = false;
                    response.Message = "No tienes un turno abierto para cerrar.";
                    response.Data = false;
                    return response;
                }

                openInventory.IsClosed = true;
                openInventory.ClosedAt = DateTime.UtcNow;

                await _inventoryRepository.UpdateInventoryAsync(openInventory);

                response.Data = true;
                response.Status = true;
                response.Message = "Turno cerrado exitosamente.";
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = "Error al cerrar turno: " + ex.Message;
                response.Data = false;
            }

            return response;
        }

        public async Task<ServiceResponseDto<EmployeeInventoryDto?>> GetOpenInventoryByEmployeeAsync(int employeeId)
        {
            var response = new ServiceResponseDto<EmployeeInventoryDto?>();

            try
            {
                var inventoryDoc = await _inventoryRepository.GetOpenInventoryAsync(employeeId);

                if (inventoryDoc == null)
                {
                    response.Data = null;
                    response.Status = true;
                    response.Message = "No hay turno activo.";
                }
                else
                {
                    response.Data = new EmployeeInventoryDto
                    {
                        EmployeeId = inventoryDoc.EmployeeId,
                        Products = inventoryDoc.Products.Select(p => new StockItemDto
                        {
                            ProductId = p.ProductId,
                            ProductName = p.ProductName,
                            Quantity = p.Quantity
                        }).ToList()
                    };
                    response.Status = true;
                    response.Message = "Inventario encontrado.";
                }
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = "Error al obtener inventario: " + ex.Message;
            }

            return response;
        }
    }
}
