using AutoMapper;
using RangoAgil.Api.DTOs;
using RangoAgil.Api.Entities;

namespace RangoAgil.Api.Profiles;

public class RangoAgilProfile : Profile
{
    public RangoAgilProfile()
    {
        CreateMap<Rango, RangoDTO>().ReverseMap();
        CreateMap<Rango, CreateRangoDTO>().ReverseMap();
        CreateMap<Rango, UpdateRangoDTO>().ReverseMap();
        CreateMap<Ingredient, IngredientDTO>()
            .ForMember(
                d => d.RangoId,
                o => o.MapFrom(s => s.Rangos.First().Id)
             );
    }
}
