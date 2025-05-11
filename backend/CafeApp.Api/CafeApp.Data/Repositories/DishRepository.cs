using CafeApp.Data.Entities;
using CafeApp.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CafeApp.Data.Repositories;

public class DishRepository : IDishRepository
{
    private readonly CafeAppDbContext _context;

    public DishRepository(CafeAppDbContext context)
    {
        _context = context;
    }
    
    public async Task<Guid> AddAsync(DishEntity dish)
    {
        dish.Id = Guid.NewGuid();
        await _context.Dishes.AddAsync(dish);
        await _context.SaveChangesAsync();

        return dish.Id;
    }
    
    public async Task<List<DishEntity>> GetAllAsync(int skip, int take, Guid cafeId)
    {
        var dishes = await _context.Dishes
            .Where(d => !d.IsDeleted && d.CafeId == cafeId)
            .Skip(skip)
            .Take(take)
            .ToListAsync();
        
        return EnsureDishExist(dishes);
    }

    public async Task<List<DishEntity>> GetByNameAsync(string name, Guid cafeId)
    {
        var dish = await _context.Dishes
            .Where(d =>
                !d.IsDeleted
                && d.CafeId == cafeId
                && d.Name.Contains(name)) //LIKE is not needed, logic is simple and does not require [] or !
            .ToListAsync();

        return EnsureDishExist(dish, name);
    }
    
    public async Task<DishEntity> GetByIdAsync(Guid id)
    {
        var dish = await _context.Dishes.FindAsync(id);

        if (dish == null)
        {
            throw new KeyNotFoundException($"Dish with ID {id} not found.");
        }

        return dish;
    }
    
    public async Task UpdateAsync(DishEntity updatedDish)
    {
        var dish = await GetByIdAsync(updatedDish.Id);

        if (IsNameChangedAndUpdated(dish, updatedDish) || IsPriceChangedAndUpdated(dish, updatedDish) ||
            IsDescriptionChangedAndUpdated(dish, updatedDish))
        {
            Console.WriteLine();
            _context.Dishes.Update(dish);
            await _context.SaveChangesAsync();
        }
        else
        {
            throw new InvalidOperationException("Dish was not updated.");
        }
    }
    
    public async Task DeleteAsync(Guid id)
    {
        var dish = await GetByIdAsync(id);

        dish.IsDeleted = true;
        _context.Dishes.Update(dish);
        await _context.SaveChangesAsync();
    }
    
    private bool IsNameChangedAndUpdated(DishEntity dish, DishEntity updatedDish)
    {
        if (dish.Name == updatedDish.Name) return false;

        dish.Name = updatedDish.Name;
        return true;
    }
    
    private bool IsPriceChangedAndUpdated(DishEntity dish, DishEntity updatedDish)
    {
        if (dish.Price == updatedDish.Price) return false;

        dish.Price = updatedDish.Price;
        return true;
    }
    
    private bool IsDescriptionChangedAndUpdated(DishEntity dish, DishEntity updatedDish)
    {
        if (dish.Description == updatedDish.Description) return false;

        dish.Description = updatedDish.Description;
        return true;
    }
    
    private List<DishEntity> EnsureDishExist(List<DishEntity> dish, string? name = null)
    {
        if (dish.Count != 0)
            return dish;

        var message = string.IsNullOrWhiteSpace(name)
            ? $"No dish with the name {name} was found."
            : "No dish found.";

        throw new InvalidOperationException(message);
    }
}