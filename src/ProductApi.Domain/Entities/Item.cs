namespace ProductApi.Domain.Entities;

public class Item
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }

    // Navigation Property
    public Product? Product { get; set; }
}
