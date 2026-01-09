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
            var result = await _inventoryService.ProductAsync(dto);

            if (!result.Status)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet("getAllProducts")]
        public async Task<IActionResult> GetAllProducts()
        {
            var result = await _inventoryService.GetAllProductsAsync();

            if (!result.Status)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPost("registerInventory")]
        public async Task<IActionResult> RegisterInventory([FromBody] EmployeeInventoryDto dto)
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdString))
            {
                return Unauthorized(new { status = false, message = "Token inválido: No se encontró el ID del usuario.", data = (object?)null });
            }

            dto.EmployeeId = int.Parse(userIdString);

            var result = await _inventoryService.RegisterInventoryAsync(dto);

            if (!result.Status)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPost("closeShift")]
        public async Task<IActionResult> CloseShift()
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdString))
                return Unauthorized(new { status = false, message = "Token inválido", data = (object?)null });

            var employeeId = int.Parse(userIdString);

            var result = await _inventoryService.CloseShiftAsync(employeeId);

            if (!result.Status)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet("get-open-inventory")]
        public async Task<IActionResult> GetOpenInventory()
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdString))
                return Unauthorized(new { status = false, message = "Token inválido", data = (object?)null });

            var employeeId = int.Parse(userIdString);

            var result = await _inventoryService.GetOpenInventoryByEmployeeAsync(employeeId);

            if (!result.Status)
            {
                return BadRequest(result);
            }

            if (result.Data == null)
            {
                return NotFound(result);
            }

            return Ok(result);
        }
    }
}
