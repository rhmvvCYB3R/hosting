namespace CafeApp.Api.Dtos.DishDtos;

public class DishDto
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }
    public Guid CafeId { get; set; }
}