using CafeApp.Data.Entities;

namespace CafeApp.Data.Repositories.Interfaces;

public interface ICafeRepository
{
    Task<Guid> AddSync(CafeEntity cafe);
    Task<List<CafeEntity>> GetAllAsync(int skip, int take);
    Task<List<CafeEntity>> GetByStreetAsync(string street);
    Task<CafeEntity> GetByIdAsync(Guid id);
    Task RateAsync(Guid id, int rating);
    Task UpdateAsync(CafeEntity updatedCafe);
    Task DeleteAsync(Guid id);
    
}