using AutoMapper;
using CafeApp.Api.Dtos.CafeDtos;
using CafeApp.BusinessLogic.Models;
using CafeApp.BusinessLogic.Services.Interfaces;
using CafeApp.Data.Entities.Enums;
using Microsoft.AspNetCore.Mvc;

namespace CafeApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CafeController : ControllerBase
{
    private readonly ICafeService _cafeService;
    private readonly IMapper _mapper;

    public CafeController(ICafeService cafeService, IMapper mapper)
    {
        _cafeService = cafeService;
        _mapper = mapper;
    }

    [HttpPost]
    [ProducesResponseType(typeof(Guid), 201)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<Guid>> CreateAsync([FromBody] CreateCafeDto dto)
    {
        var model = _mapper.Map<Cafe>(dto);
        var id = await _cafeService.AddAsync(model);
        return Created ($"/api/cafe/{id}", id);
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<CafeDto>), 200)]
    public async Task<ActionResult<List<CafeDto>>> GetAllAsync([FromQuery] int skip = 0, [FromQuery] int take = 10)
    {
        var cafes = await _cafeService.GetAllAsync(skip, take);
        var result = _mapper.Map<List<CafeDto>>(cafes);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(CafeDto), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<CafeDto>> GetByIdAsync(Guid id)
    {
        var cafe = await _cafeService.GetByIdAsync(id);
        var result = _mapper.Map<CafeDto>(cafe);
        return Ok(result);
    }

    [HttpGet("by-street")]
    [ProducesResponseType(typeof(List<CafeDto>), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<List<CafeDto>>> GetByStreetAsync([FromQuery] string street)
    {
        var cafe = await _cafeService.GetByStreetAsync(street);
        var result = _mapper.Map<List<CafeDto>>(cafe);
        return Ok(result);
    }

    [HttpPost("{id:guid}/rate")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public async Task<ActionResult> RateAsync(Guid id, [FromQuery] RatingEnum rating)
    {
        await _cafeService.RateAsync(id, rating);
        return NoContent();
    }

    [HttpPut("update")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public async Task<ActionResult> UpdateAsync([FromBody] UpdateCafeDto dto)
    {
        var model = _mapper.Map<Cafe>(dto);
        await _cafeService.UpdateAsync(model);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public async Task<ActionResult> DeleteAsync(Guid id)
    {
        await _cafeService.DeleteAsync(id);
        return NoContent();
    }
}