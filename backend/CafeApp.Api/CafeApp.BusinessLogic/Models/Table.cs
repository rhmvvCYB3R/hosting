namespace CafeApp.BusinessLogic.Models;

public class Table
{
    public Guid Id { get; set; }
    public int Number { get; set; }
    public int Seats { get; set; }
    public DateTime? ReservedUntil { get; set; }

    public Guid CafeId { get; set; }
}