using CafeApp.Data.Entities.Enums;

namespace CafeApp.BusinessLogic.Models;

public class Cafe
{
    public Guid Id { get; set; }
    
    public string Street { get; set; }
    public string City { get; set; }
    
    //time is stored in ticks (100-nanosecond intervals) from midnight (00:00:00) of the current day.
    //8:00 AM = 288,000,000,000 ticks, 11:00 PM = 828,000,000,000 ticks.
    public RatingEnum Rating { get; set; }
    public TimeSpan OpeningTime { get; set; }
    public TimeSpan ClosingTime { get; set; }
    
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
}