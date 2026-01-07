using backend.Domain.Documents;

namespace backend.Application.DTOs
{
    public class EmployeeInventoryDto
    {
        public int EmployeeId { get; set; }

        public List<StockItemDto> Products { get; set; } = new List<StockItemDto>();
    }

    public class StockItemDto
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public int Quantity { get; set; }
    }
}
