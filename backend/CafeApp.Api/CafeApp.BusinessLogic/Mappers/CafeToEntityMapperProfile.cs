using AutoMapper;
using CafeApp.BusinessLogic.Models;
using CafeApp.Data.Entities;

namespace CafeApp.BusinessLogic.Mappers;

public class CafeToEntityMapperProfile : Profile
{
    public CafeToEntityMapperProfile()
        => CreateMap<Cafe, CafeEntity>().ReverseMap();
}