using ProductApi.Application.DTOs;
using ProductApi.Domain.Entities;

namespace ProductApi.Application.Interfaces;

// ── Repository Interfaces ───────────────────────────────────────

public interface IProductRepository
{
    Task<(List<Product> Items, int TotalCount)> GetAllAsync(int pageNumber, int pageSize);
    Task<Product?> GetByIdAsync(int id);
    Task<Product?> GetByIdWithItemsAsync(int id);
    Task AddAsync(Product product);
    void Update(Product product);
    void Delete(Product product);
    Task<bool> SaveChangesAsync();
}

public interface IItemRepository
{
    Task<List<Item>> GetByProductIdAsync(int productId);
    Task<bool> ProductExistsAsync(int productId);
    Task AddAsync(Item item);
    Task<bool> SaveChangesAsync();
}

// ── Service Interfaces ──────────────────────────────────────────

public interface IProductService
{
    Task<PagedResult<ProductReadDto>> GetAllAsync(int pageNumber, int pageSize);
    Task<ProductReadDto> GetByIdAsync(int id);
    Task<ProductReadDto> CreateAsync(ProductCreateDto dto, string createdBy);
    Task<ProductReadDto> UpdateAsync(int id, ProductUpdateDto dto, string modifiedBy);
    Task DeleteAsync(int id);
}

public interface IItemService
{
    Task<List<ItemReadDto>> GetByProductIdAsync(int productId);
    Task<ItemReadDto> CreateAsync(ItemCreateDto dto);
}

public interface ITokenService
{
    (string Token, DateTime ExpiresAt) GenerateToken(string username);
}
