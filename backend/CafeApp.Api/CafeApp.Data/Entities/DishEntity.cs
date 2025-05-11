namespace CafeApp.Data.Entities;

public class DishEntity
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }
    
    public bool IsDeleted { get; set; }
    
    public Guid CafeId { get; set; }
    public CafeEntity Cafe { get; set; }
}