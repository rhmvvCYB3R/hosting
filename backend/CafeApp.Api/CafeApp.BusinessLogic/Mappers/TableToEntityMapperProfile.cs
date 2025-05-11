using AutoMapper;
using CafeApp.BusinessLogic.Models;
using CafeApp.Data.Entities;

namespace CafeApp.BusinessLogic.Mappers;

public class TableToEntityMapperProfile : Profile
{
    public TableToEntityMapperProfile()
        => CreateMap<Table, TableEntity>().ReverseMap();
}