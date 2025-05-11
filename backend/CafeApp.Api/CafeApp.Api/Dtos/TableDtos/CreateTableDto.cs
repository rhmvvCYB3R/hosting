namespace CafeApp.Api.Dtos.TableDtos;

public class CreateTableDto
{
    public int Number { get; set; }
    public int Seats { get; set; }
    public DateTime? ReservedUntil { get; set; }
    public Guid CafeId { get; set; }
}