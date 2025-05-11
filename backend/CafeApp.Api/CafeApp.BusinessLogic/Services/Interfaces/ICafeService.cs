using CafeApp.BusinessLogic.Models;
using CafeApp.Data.Entities.Enums;

namespace CafeApp.BusinessLogic.Services.Interfaces;

public interface ICafeService
{
    Task<Guid> AddAsync(Cafe model);
    Task<List<Cafe>> GetAllAsync(int skip, int take);
    Task<Cafe> GetByIdAsync(Guid id);
    Task<List<Cafe>> GetByStreetAsync(string street);
    Task RateAsync(Guid id, RatingEnum rating);
    Task UpdateAsync(Cafe model);
    Task DeleteAsync(Guid id);
}
