using CafeApp.Data.Entities.Enums;

namespace CafeApp.Api.Dtos.CafeDtos;

public class CafeDto
{
        public Guid Id { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public RatingEnum Rating { get; set; }
        public TimeSpan OpeningTime { get; set; }
        public TimeSpan ClosingTime { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public DateTime CreatedAt { get; set; }
}