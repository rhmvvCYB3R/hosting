using AutoMapper;
using CafeApp.BusinessLogic.Models;
using CafeApp.Data.Entities;

namespace CafeApp.BusinessLogic.Mappers;

public class DishToEntityMapperProfile : Profile
{
    public DishToEntityMapperProfile()
        => CreateMap<Dish, DishEntity>().ReverseMap(); 
}