using CafeApp.Data.Entities;

namespace CafeApp.Data.Repositories.Interfaces;

public interface IDishRepository
{
    Task<Guid> AddAsync(DishEntity dish);
    Task<List<DishEntity>> GetAllAsync(int skip, int take, Guid cafeId);
    Task<List<DishEntity>> GetByNameAsync(string name, Guid cafeId);
    Task<DishEntity> GetByIdAsync(Guid id);
    Task UpdateAsync(DishEntity updatedDish);
    Task DeleteAsync(Guid id);
}