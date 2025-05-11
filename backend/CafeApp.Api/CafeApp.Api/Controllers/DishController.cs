using AutoMapper;
using CafeApp.Api.Dtos.DishDtos;
using CafeApp.BusinessLogic.Models;
using CafeApp.BusinessLogic.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CafeApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DishController : ControllerBase
{
    private readonly IDishService _service;
    private readonly IMapper _mapper;

    public DishController(IMapper mapper, IDishService service)
    {
        _mapper = mapper;
        _service = service;
    }
    
    [HttpGet("{id:guid}")] 
    [ProducesResponseType(typeof(DishDto), 200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<DishDto>> GetByIdAsync(Guid id)
    {
        var dish = await _service.GetByIdAsync(id);
        var result = _mapper.Map<DishDto>(dish);
        return Ok(result);
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(List<DishDto>), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<List<DishDto>>> GetAllAsync([FromQuery] Guid cafeId, [FromQuery] int skip = 0, [FromQuery] int take = 10)
    {
        var dishes = await _service.GetAllAsync(skip, take, cafeId);
        var result = _mapper.Map<List<DishDto>>(dishes);
        return Ok(result);
    }
    
    [HttpGet("by-name")]
    [ProducesResponseType(typeof(List<DishDto>), 200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<List<DishDto>>> GetByNameAsync([FromQuery] string name, [FromQuery] Guid cafeId)
    {
        var dishes = await _service.GetByNameAsync(name, cafeId);
        var result = _mapper.Map<List<DishDto>>(dishes);
        return Ok(result);
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(DishDto), 200)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<DishDto>> CreateAsync([FromBody] CreateDishDto dto)
    {
        var model = _mapper.Map<Dish>(dto);
        
        var id = await _service.AddAsync(model);
        return Created($"/api/dish/{id}", id);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(DishDto), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<DishDto>> UpdateAsync([FromBody] UpdateDishDto dto)
    {
        var model = _mapper.Map<Dish>(dto);
        await _service.UpdateAsync(model);
        
        return NoContent();
    }
    
    [HttpDelete("{id:guid}")] 
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<ActionResult> DeleteAsync(Guid id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }
}