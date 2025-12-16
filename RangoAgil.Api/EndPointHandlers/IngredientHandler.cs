using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RangoAgil.Api.DTOs;
using RangoAgil.Api.Entities;
using RangoAgil.Api.RangoDbContext;

namespace RangoAgil.Api.EndPointHandlers;

public static class IngredientHandler
{
    public static async Task<Results<NoContent, Ok<List<IngredientDTO>>>> GetIngredientsAsync(
        RangoContext context,
        IMapper mapper,
        int rangoId)   
    {
        var result = mapper.Map<IEnumerable<IngredientDTO>>((await context.Rangos
                            .Include(rango => rango.Ingredients)
                            .FirstOrDefaultAsync(rango => rango.Id == rangoId))?.Ingredients).ToList();


        if (!result.Any() || result == null)
            return TypedResults.NoContent();
        else
            return TypedResults.Ok(result);
    }
}
