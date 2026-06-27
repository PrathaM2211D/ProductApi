namespace ProductApi.Application.DTOs;

// ── Product ─────────────────────────────────────────────────────

public class ProductCreateDto
{
    public string ProductName { get; set; } = string.Empty;
}

public class ProductUpdateDto
{
    public string ProductName { get; set; } = string.Empty;
}

public class ProductReadDto
{
    public int Id { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime CreatedOn { get; set; }
    public string? ModifiedBy { get; set; }
    public DateTime? ModifiedOn { get; set; }
    public List<ItemReadDto> Items { get; set; } = new();
}

public class PagedResult<T>
{
    public List<T> Items { get; set; } = new();
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
}

// ── Item ────────────────────────────────────────────────────────

public class ItemCreateDto
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}

public class ItemReadDto
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}

// ── Auth ────────────────────────────────────────────────────────

public class LoginRequestDto
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class LoginResponseDto
{
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
}
