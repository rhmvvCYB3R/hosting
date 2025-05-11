namespace CafeApp.BusinessLogic.Models;

public class Dish
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }
    
    public Guid CafeId { get; set; }
}