using AutoMapper;
using CafeApp.Api.Dtos.CafeDtos;
using CafeApp.BusinessLogic.Models;

namespace CafeApp.Api.Dtos.Mappers;

public class CafeDtoMapperProfile : Profile
{
    public CafeDtoMapperProfile()
    {
        CreateMap<CreateCafeDto, Cafe>().ReverseMap();
        CreateMap<UpdateCafeDto, Cafe>().ReverseMap();
        CreateMap<Cafe, CafeDto>().ReverseMap();
    }
}