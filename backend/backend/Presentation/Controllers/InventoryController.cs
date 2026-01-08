using backend.Application.DTOs;
using backend.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace backend.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;

        public InventoryController(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        [HttpPost("entryProduct")]
        public async Task<IActionResult> EntryProduct([FromBody] ProductDto dto)
        {
            try
            {
                var product = await _inventoryService.ProductAsync(dto);
                return Ok(new { message = "Producto registrado correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("getAllProducts")]
        public async Task<IActionResult> GetAllProducts()
        {
            try
            {
                var products = await _inventoryService.GetAllProductsAsync();
                return Ok(products);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("registerInventory")]
        public async Task<IActionResult> RegisterInventory([FromBody] EmployeeInventoryDto dto)
        {
            try
            {
                var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userIdString))
                {
                    return Unauthorized(new { message = "Token inválido: No se encontró el ID del usuario." });
                }

                dto.EmployeeId = int.Parse(userIdString);

                await _inventoryService.RegisterInventoryAsync(dto);

                return Ok(new { message = "Inventario actualizado con éxito" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("closeShift")]
        public async Task<IActionResult> CloseShift()
        {
            try
            {
                var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdString)) return Unauthorized(new { message = "Token inválido" });

                var employeeId = int.Parse(userIdString);

                await _inventoryService.CloseShiftAsync(employeeId);

                return Ok(new { message = "Turno cerrado correctamente. Hasta mañana." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("get-open-inventory")]
        public async Task<IActionResult> GetOpenInventory()
        {
            try
            {
                var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdString)) return Unauthorized(new { message = "Token inválido" });

                var employeeId = int.Parse(userIdString);

                var inventory = await _inventoryService.GetOpenInventoryByEmployeeAsync(employeeId);

                if (inventory == null)
                {
                    return NotFound(new { message = "No tienes un turno abierto actualmente." });
                }

                return Ok(inventory);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
