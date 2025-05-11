using AutoMapper;
using CafeApp.BusinessLogic.Models;
using CafeApp.BusinessLogic.Services.Interfaces;
using CafeApp.Data.Entities;
using CafeApp.Data.Repositories.Interfaces;

namespace CafeApp.BusinessLogic.Services;

public class DishService : IDishService
{
    private readonly IDishRepository _repository;
    private readonly IMapper _mapper;

    public DishService(IDishRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    
    public async Task<Guid> AddAsync(Dish model)
    {
        var entity = _mapper.Map<DishEntity>(model);
        return await _repository.AddAsync(entity);
    }

    public async Task<Dish> GetByIdAsync(Guid id)
    {
        if (id.Equals(Guid.Empty))
        {
            throw new ArgumentException("Id cannot be empty.");
        }
        var entity = await _repository.GetByIdAsync(id);
        return _mapper.Map<Dish>(entity);
    }
    
    public async Task<List<Dish>> GetAllAsync(int skip, int take, Guid cafeId)
    {
        if (cafeId == Guid.Empty || skip < 0 || take < 0)
            throw new ArgumentException("Invalid input.");

        var entities = await _repository.GetAllAsync(skip, take, cafeId);
        return _mapper.Map<List<Dish>>(entities);
    }

    public async Task<List<Dish>> GetByNameAsync(string name, Guid cafeId)
    {
        if (string.IsNullOrWhiteSpace(name) || cafeId == Guid.Empty)
            throw new ArgumentException("Invalid input.");

        var entities = await _repository.GetByNameAsync(name, cafeId);
        return _mapper.Map<List<Dish>>(entities);
    }

    
    public async Task UpdateAsync(Dish model)
    {
        if (model.Id.Equals(Guid.Empty))
        {
            throw new ArgumentException("Id cannot be empty.");
        }
        var entity = _mapper.Map<DishEntity>(model);
        await _repository.UpdateAsync(entity);
    }
    
    public async Task DeleteAsync(Guid id)
    {
        if (id.Equals(Guid.Empty))
        {
            throw new ArgumentException("Id cannot be empty.");
        }
        await _repository.DeleteAsync(id);
    }
}