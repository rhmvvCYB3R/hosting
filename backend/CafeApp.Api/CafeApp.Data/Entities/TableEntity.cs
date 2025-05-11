namespace CafeApp.Data.Entities;

public class TableEntity
{
    public Guid Id { get; set; }
    public int Number { get; set; }
    public int Seats { get; set; }
    public DateTime? ReservedUntil { get; set; }
    
    public bool IsDeleted { get; set; }

    public Guid CafeId { get; set; }
    public CafeEntity Cafe { get; set; }
}