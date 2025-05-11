namespace CafeApp.Api.Dtos.TableDtos;

public class UpdateTableDto
{
    public int Number { get; set; }
    public int Seats { get; set; }
    public DateTime? ReservedUntil { get; set; }
}