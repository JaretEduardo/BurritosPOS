using backend.Domain.Entities;

namespace backend.Application.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();

        Task AddAsync(Product product);
    }
}
