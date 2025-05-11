using AutoMapper;
using CafeApp.BusinessLogic.Models;
using CafeApp.BusinessLogic.Services.Interfaces;
using CafeApp.Data.Entities;
using CafeApp.Data.Entities.Enums;
using CafeApp.Data.Repositories.Interfaces;

namespace CafeApp.BusinessLogic.Services;

public class CafeService : ICafeService
{
    private readonly ICafeRepository _cafeRepository;
    private readonly IMapper _mapper;

    public CafeService(ICafeRepository cafeRepository, IMapper mapper)
    {
        _cafeRepository = cafeRepository;
        _mapper = mapper;
    }

    public async Task<Guid> AddAsync(Cafe model)
    {
        var entity = _mapper.Map<CafeEntity>(model);
        return await _cafeRepository.AddSync(entity);
    }

    public async Task<List<Cafe>> GetAllAsync(int skip, int take)
    {      
        if (skip < 0 || take < 0)
            throw new ArgumentException("Invalid input.");

        var entities = await _cafeRepository.GetAllAsync(skip, take);
        return _mapper.Map<List<Cafe>>(entities);
    }
    
    public async Task<Cafe> GetByIdAsync(Guid id)
    {
        if (id.Equals(Guid.Empty))
        {
            throw new ArgumentException("Id cannot be empty.");
        }
        var entity = await _cafeRepository.GetByIdAsync(id);
        return _mapper.Map<Cafe>(entity);
    }
    
    public async Task<List<Cafe>> GetByStreetAsync(string street)
    {
        if (string.IsNullOrWhiteSpace(street))
        {
            throw new ArgumentException("Street cannot be null or whitespace.");
        }
        
        var entity = await _cafeRepository.GetByStreetAsync(street);
        return _mapper.Map<List<Cafe>>(entity);
    }
    
    public async Task RateAsync(Guid id, RatingEnum rating)
    {
        if (id.Equals(Guid.Empty))
        {
            throw new ArgumentException("Id cannot be empty.");
        }
        await _cafeRepository.RateAsync(id, (int)rating);
    }
    
    public async Task UpdateAsync(Cafe model)
    {
        if (model.Id.Equals(Guid.Empty))
        {
            throw new ArgumentException("Id cannot be empty.");
        }
        var entity = _mapper.Map<CafeEntity>(model);
        await _cafeRepository.UpdateAsync(entity);
    }
    
    public async Task DeleteAsync(Guid id)
    {
        if (id.Equals(Guid.Empty))
        {
            throw new ApplicationException("Id cannot be empty.");
        }
        await _cafeRepository.DeleteAsync(id);
    }
}