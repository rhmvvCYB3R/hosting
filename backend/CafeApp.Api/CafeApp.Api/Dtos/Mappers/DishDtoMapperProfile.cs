using AutoMapper;
using CafeApp.Api.Dtos.DishDtos;
using CafeApp.BusinessLogic.Models;

namespace CafeApp.Api.Dtos.Mappers;

public class DishDtoMapperProfile : Profile
{
    public DishDtoMapperProfile()
    {
        CreateMap<Dish, DishDto>().ReverseMap();
        CreateMap<Dish, CreateDishDto>().ReverseMap();
        CreateMap<Dish, UpdateDishDto>().ReverseMap();
    }
}