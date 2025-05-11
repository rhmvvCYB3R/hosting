using CafeApp.Data.Entities;

namespace CafeApp.Data.Repositories.Interfaces;

public interface ITableRepository
{
    Task<Guid> AddASync(TableEntity table);
    Task<List<TableEntity>> GetAllAsync(int skip, int take, Guid cafeId);
    Task<TableEntity> GetByNumberAsync(int number, Guid cafeId);
    Task<TableEntity> GetByIdAsync(Guid id);
    Task ReserveUntilAsync(Guid id, DateTime until);
    Task UpdateAsync(Guid id, TableEntity updatedTable);
    Task DeletedAsync(Guid id);
}