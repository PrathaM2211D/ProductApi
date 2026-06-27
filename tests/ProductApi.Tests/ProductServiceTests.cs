using Moq;
using ProductApi.Application.DTOs;
using ProductApi.Application.Interfaces;
using ProductApi.Application.Services;
using ProductApi.Domain.Entities;
using ProductApi.Domain.Exceptions;

namespace ProductApi.Tests;

public class ProductServiceTests
{
    private readonly Mock<IProductRepository> _repoMock;
    private readonly ProductService _service;

    public ProductServiceTests()
    {
        _repoMock = new Mock<IProductRepository>();
        _service = new ProductService(_repoMock.Object);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsProduct_WhenProductExists()
    {
        var product = new Product { Id = 1, ProductName = "Laptop", CreatedBy = "tester", CreatedOn = DateTime.UtcNow };
        _repoMock.Setup(r => r.GetByIdWithItemsAsync(1)).ReturnsAsync(product);

        var result = await _service.GetByIdAsync(1);

        Assert.Equal("Laptop", result.ProductName);
        Assert.Equal("tester", result.CreatedBy);
    }

    [Fact]
    public async Task GetByIdAsync_ThrowsNotFoundException_WhenProductDoesNotExist()
    {
        _repoMock.Setup(r => r.GetByIdWithItemsAsync(99)).ReturnsAsync((Product?)null);

        await Assert.ThrowsAsync<NotFoundException>(() => _service.GetByIdAsync(99));
    }

    [Fact]
    public async Task CreateAsync_AddsProductAndReturnsDto()
    {
        var dto = new ProductCreateDto { ProductName = "Keyboard" };
        _repoMock.Setup(r => r.AddAsync(It.IsAny<Product>())).Returns(Task.CompletedTask);
        _repoMock.Setup(r => r.SaveChangesAsync()).ReturnsAsync(true);

        var result = await _service.CreateAsync(dto, "tester");

        Assert.Equal("Keyboard", result.ProductName);
        Assert.Equal("tester", result.CreatedBy);
        _repoMock.Verify(r => r.AddAsync(It.IsAny<Product>()), Times.Once);
        _repoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ThrowsNotFoundException_WhenProductDoesNotExist()
    {
        _repoMock.Setup(r => r.GetByIdAsync(5)).ReturnsAsync((Product?)null);
        var dto = new ProductUpdateDto { ProductName = "Updated" };

        await Assert.ThrowsAsync<NotFoundException>(() => _service.UpdateAsync(5, dto, "editor"));
    }

    [Fact]
    public async Task UpdateAsync_UpdatesProduct_WhenProductExists()
    {
        var product = new Product { Id = 1, ProductName = "Old Name", CreatedBy = "tester", CreatedOn = DateTime.UtcNow };
        _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(product);
        _repoMock.Setup(r => r.SaveChangesAsync()).ReturnsAsync(true);

        var result = await _service.UpdateAsync(1, new ProductUpdateDto { ProductName = "New Name" }, "editor");

        Assert.Equal("New Name", result.ProductName);
        Assert.Equal("editor", result.ModifiedBy);
    }

    [Fact]
    public async Task DeleteAsync_RemovesProduct_WhenProductExists()
    {
        var product = new Product { Id = 1, ProductName = "ToDelete", CreatedBy = "tester", CreatedOn = DateTime.UtcNow };
        _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(product);
        _repoMock.Setup(r => r.SaveChangesAsync()).ReturnsAsync(true);

        await _service.DeleteAsync(1);

        _repoMock.Verify(r => r.Delete(product), Times.Once);
        _repoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task GetAllAsync_NormalizesInvalidPageValues()
    {
        _repoMock.Setup(r => r.GetAllAsync(1, 10)).ReturnsAsync((new List<Product>(), 0));

        var result = await _service.GetAllAsync(pageNumber: 0, pageSize: 500);

        Assert.Equal(1, result.PageNumber);
        Assert.Equal(10, result.PageSize);
    }
}
