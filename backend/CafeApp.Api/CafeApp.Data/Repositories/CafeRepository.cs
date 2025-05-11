using CafeApp.Data.Entities;
using CafeApp.Data.Entities.Enums;
using CafeApp.Data.Repositories.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace CafeApp.Data.Repositories;

public class CafeRepository : ICafeRepository
{
    private readonly CafeAppDbContext _context;

    public CafeRepository(CafeAppDbContext context)
    {
        _context = context;
    }
    
    public async Task<Guid> AddSync(CafeEntity cafe)
    {
        cafe.Id = Guid.NewGuid();
        cafe.CreatedAt = DateTime.UtcNow;
        
        await _context.Cafes.AddAsync(cafe);
        await _context.SaveChangesAsync();

        return cafe.Id;
    }

    public async Task<List<CafeEntity>> GetAllAsync(int skip, int take)
    {
        var cafes = await _context.Cafes
            .Where(c => !c.IsDeleted)
            .Skip(skip)
            .Take(take)
            .ToListAsync();
        
        return EnsureCafeExist(cafes);
    }

    public async Task<List<CafeEntity>> GetByStreetAsync(string street)
    {
        var cafe = await _context.Cafes
            .Where(c => !c.IsDeleted && EF.Functions.Like(c.Street, $"%{street}%"))
            .ToListAsync();

        return EnsureCafeExist(cafe, street);
    }

    public async Task<CafeEntity> GetByIdAsync(Guid id)
    {
        var cafe = await _context.Cafes.FindAsync(id);

        if (cafe == null)
        {
            throw new KeyNotFoundException($"Cafe with ID {id} not found.");
        }

        return cafe;
    }

    public async Task RateAsync(Guid id, int rating)
    {
        if (rating is < 1 or > 5)
            throw new ArgumentOutOfRangeException(nameof(rating), "Rating must be between 1 and 5.");

        var cafe = await _context.Cafes.FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);
        if (cafe is null)
            throw new KeyNotFoundException($"Cafe with ID {id} not found.");

        var newRatingCount = cafe.RatingCount + 1;
        var newAverageRating = ((double)cafe.Rating * cafe.RatingCount + rating) / newRatingCount;

        cafe.Rating = (RatingEnum)Math.Round(newAverageRating);
        cafe.RatingCount = newRatingCount;

        await _context.SaveChangesAsync();

    }


    public async Task UpdateAsync(CafeEntity updatedCafe)
    {
        var cafe = await GetByIdAsync(updatedCafe.Id);

        if (IsStreetChangedAndUpdated(cafe, updatedCafe) || IsCityChangedAndUpdated(cafe, updatedCafe) ||
            IsRatingChangedAndUpdated(cafe, updatedCafe) || IsOpeningTimeChangedAndUpdated(cafe, updatedCafe) ||
            IsClosingTimeChangedAndUpdated(cafe, updatedCafe))
        {
            _context.Cafes.Update(cafe);
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteAsync(Guid id)
    {
        var cafe = await GetByIdAsync(id);

        cafe.IsDeleted = true;
        _context.Cafes.Update(cafe);
        await _context.SaveChangesAsync();
    }
    
    private bool IsStreetChangedAndUpdated(CafeEntity cafe, CafeEntity updatedCafe)
    {
        if (cafe.Street == updatedCafe.Street) return false;

        cafe.Street = updatedCafe.Street;
        return true;
    }

    private bool IsCityChangedAndUpdated(CafeEntity cafe, CafeEntity updatedCafe)
    {
        if (cafe.City == updatedCafe.City) return false;

        cafe.City = updatedCafe.City;
        return true;
    }
  
    private bool IsRatingChangedAndUpdated(CafeEntity cafe, CafeEntity updatedCafe)
    {
        if (cafe.Rating == updatedCafe.Rating) return false;

        cafe.Rating = updatedCafe.Rating;
        return true;
    }

    private bool IsOpeningTimeChangedAndUpdated(CafeEntity cafe, CafeEntity updatedCafe)
    {
        if (cafe.OpeningTime == updatedCafe.OpeningTime) return false;

        cafe.OpeningTime = updatedCafe.OpeningTime;
        return true;
    }

    private bool IsClosingTimeChangedAndUpdated(CafeEntity cafe, CafeEntity updatedCafe)
    {
        if (cafe.ClosingTime == updatedCafe.ClosingTime) return false;

        cafe.ClosingTime = updatedCafe.ClosingTime;
        return true;
    }

    private List<CafeEntity> EnsureCafeExist(List<CafeEntity> cafe, string? street = null)
    {
        if (cafe.Count != 0)
            return cafe;

        var message = street != null
            ? $"No cafe was found on this address {street}."
            : "No cafe found.";

        throw new InvalidOperationException(message);
    }
}



