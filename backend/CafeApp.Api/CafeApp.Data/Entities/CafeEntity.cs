using CafeApp.Data.Entities.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace CafeApp.Data.Entities;

public class CafeEntity
{
    public Guid Id { get; set; }
    
    public string Street { get; set; }
    public string City { get; set; }
    
    public RatingEnum Rating { get; set; }
    public int RatingCount { get; set; }
    public TimeSpan OpeningTime { get; set; }
    public TimeSpan ClosingTime { get; set; }
    
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public bool IsDeleted { get; set; }
    
    public ICollection<TableEntity> Tables { get; set; }
    public ICollection<DishEntity> Dishes { get; set; }

}
