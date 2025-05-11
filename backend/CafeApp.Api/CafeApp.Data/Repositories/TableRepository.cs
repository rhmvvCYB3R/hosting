using CafeApp.Data.Entities;
using CafeApp.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CafeApp.Data.Repositories;

public class TableRepository : ITableRepository
{
    private readonly CafeAppDbContext _context;

    public TableRepository(CafeAppDbContext context)
    {
        _context = context;
    }
    
    public async Task<Guid> AddASync(TableEntity table)
    {
        table.Id = Guid.NewGuid();
        await _context.Tables.AddAsync(table);
        await _context.SaveChangesAsync();

        return table.Id;
    }
    
    public async Task<List<TableEntity>> GetAllAsync(int skip, int take, Guid cafeId)
    {
        var tables = await _context.Tables
            .Where(t => !t.IsDeleted && t.CafeId == cafeId)
            .Skip(skip)
            .Take(take)
            .ToListAsync();
        
        return EnsureTableExist(tables);
    }
    
    public async Task<TableEntity> GetByNumberAsync(int number, Guid cafeId)
    {
        var table = await _context.Tables
            .FirstOrDefaultAsync(t =>
                !t.IsDeleted
                && t.CafeId == cafeId
                && EF.Functions.Like(t.Number.ToString(), $"%{number}%"));
        
        return table ?? throw new KeyNotFoundException($"Table from cafe {cafeId} with number {number} not found.");
    }
    
    public async Task<TableEntity> GetByIdAsync(Guid id)
    {
        var table = await _context.Tables.FindAsync(id);

        if (table == null)
        {
            throw new KeyNotFoundException($"Table with ID {id} not found.");
        }

        return table;
    }

    public async Task ReserveUntilAsync(Guid id, DateTime until)
    {
        var table = await _context.Tables.FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted);
        if (table is null)
            throw new KeyNotFoundException($"Table with ID {id} not found.");

        if (table.ReservedUntil >= DateTime.UtcNow)
            throw new InvalidOperationException("Table is already reserved.");

        table.ReservedUntil = until;
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Guid id, TableEntity updatedTable)
    {
        var table = await GetByIdAsync(id);

        if (IsNumberChangedAndUpdated(table, updatedTable) || IsSeatsChangedAndUpdated(table, updatedTable))
        {
            Console.WriteLine();
            _context.Tables.Update(table);
            await _context.SaveChangesAsync();
        }
    }
    
    public async Task DeletedAsync(Guid id)
    {
        var table = await GetByIdAsync(id);

        table.IsDeleted = true;
        _context.Tables.Update(table);
        await _context.SaveChangesAsync();
    }
    
    private bool IsNumberChangedAndUpdated(TableEntity table, TableEntity updatedTable)
    {
        if (table.Number == updatedTable.Number) return false;

        table.Number = updatedTable.Number;
        return true;
    }
    
    private bool IsSeatsChangedAndUpdated(TableEntity table, TableEntity updatedSeats)
    {
        if (table.Seats == updatedSeats.Seats) return false;

        table.Seats = updatedSeats.Seats;
        return true;
    }
    
    private List<TableEntity> EnsureTableExist(List<TableEntity> table, string? number = null)
    {
        if (table.Count != 0)
            return table;

        var message = number != null
            ? $"Table number {number} not found."
            : "Table does not exist.";

        throw new InvalidOperationException(message);
    }
}