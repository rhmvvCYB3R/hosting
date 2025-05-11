using System.ComponentModel.DataAnnotations;
using CafeApp.Data.Entities.Enums;

namespace CafeApp.Api.Dtos.CafeDtos;

public class CreateCafeDto
{
    
    public string Street { get; set; }
    public string City { get; set; }
    public RatingEnum Rating { get; set; }
    public TimeSpan OpeningTime { get; set; }
    public TimeSpan ClosingTime { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
}