using CafeApp.BusinessLogic.Models;

namespace CafeApp.BusinessLogic.Services.Interfaces;

public interface IDishService
{
    Task<Guid> AddAsync(Dish model);
    Task<Dish> GetByIdAsync(Guid id);
    Task<List<Dish>> GetAllAsync(int skip, int take, Guid cafeId);
    Task<List<Dish>> GetByNameAsync(string name, Guid cafeId);
    Task UpdateAsync(Dish model);
    Task DeleteAsync(Guid id);
}
