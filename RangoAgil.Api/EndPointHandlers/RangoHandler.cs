using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RangoAgil.Api.DTOs;
using RangoAgil.Api.Entities;
using RangoAgil.Api.RangoDbContext;

namespace RangoAgil.Api.EndPointHandlers;

public static class RangoHandler
{
    public static async Task<Results<NoContent, Ok<List<RangoDTO>>>> GetRangosAsync(
        RangoContext context,
        IMapper mapper,
        [FromQuery(Name = "name")] string? name)
    {


        var result = mapper.Map<IEnumerable<RangoDTO>>(await context.Rangos
                            .Where(x => name == null || x.Name.ToLower().Contains(name.ToLower())).ToListAsync());

        if (!result.Any() || result == null)
            return TypedResults.NoContent();
        else
            return TypedResults.Ok(result.ToList());
    }

    public static async Task<Results<NoContent, Ok<RangoDTO>>> GetRangoByIdAsync(
        RangoContext context,
        IMapper mapper,
        ILogger<RangoDTO> logger,
        [FromRoute] int rangoId)
    {
        var rango = await context.Rangos.FirstOrDefaultAsync(r => r.Id == rangoId);
        if (rango == null)
        {
            logger.LogInformation($"Register not founded id: {rangoId}");
            return TypedResults.NoContent();
        }
        else
        {
            logger.LogInformation($"Register founded id: {rangoId}");
            return TypedResults.Ok(mapper.Map<RangoDTO>(rango));
        }
    }

    public static async Task<Results<NoContent, Ok<RangoDTO>>> GetRangoByNameAsync(
        RangoContext context,
        IMapper mapper,
        [FromRoute] string name)
    {
        var rango = await context.Rangos.FirstOrDefaultAsync(r => r.Name.ToLower() == name.ToLower());
        if (rango == null)
            return TypedResults.NoContent();
        else
            return TypedResults.Ok(mapper.Map<RangoDTO>(rango));
    }

    public static async Task<Results<BadRequest, Created<RangoDTO>>> CreateRangoAsync(
        RangoContext context,
        IMapper mapper,
        [FromBody] CreateRangoDTO createRangoDTO,
        LinkGenerator linkGenerator,
        ILogger<RangoDTO> logger,
        HttpContext httpContext)
    {
        var entity = mapper.Map<Rango>(createRangoDTO);
        context.Add(entity);
        try
        {
            await context.SaveChangesAsync();
            var result = mapper.Map<RangoDTO>(entity);
            var linkToReturn = linkGenerator.GetUriByName(
                 httpContext,
                "GetRangos",
                new { id = result.Id }
            );

            logger.LogInformation($"Register created id: {result.Id}");
            return TypedResults.Created(linkToReturn, result);
        }
        catch (Exception ex)
        {
            logger.LogError($"Error creating register Error: {ex.Message}");
            return TypedResults.BadRequest();
        }

    }

    public static async Task<Results<NotFound, NoContent, BadRequest>> UpdateRangoAsync(
        RangoContext context,
        IMapper mapper,
        ILogger<RangoDTO> logger,
        [FromBody] UpdateRangoDTO rangoDTO,
        [FromRoute] int rangoId)
    {
        var entity = await context.Rangos.FirstOrDefaultAsync(x => x.Id == rangoId);
        if (entity == null)
            return TypedResults.NotFound();

        mapper.Map(rangoDTO, entity);

        try
        {
            logger.LogInformation($"Register updated id: {rangoId}");
            await context.SaveChangesAsync();
            return TypedResults.NoContent();
        }
        catch (Exception ex)
        {
            logger.LogError($"Error on updating register Error: {ex.Message}");
            return TypedResults.BadRequest();
        }
        
        
    }

    public static async Task<Results<NotFound, NoContent, BadRequest>> DeleteRangoAsync(
        RangoContext context,
        ILogger<RangoDTO> logger,
        [FromRoute] int rangoId)
    {
        var entity = context.Rangos.FirstOrDefault(x => x.Id == rangoId);
        if (entity == null)
            return TypedResults.NotFound();
        context.Rangos.Remove(entity);
        try
        {

            await context.SaveChangesAsync();
            logger.LogInformation($"Register deleted id: {rangoId}");
            return TypedResults.NoContent();
        }
        catch (Exception ex)
        {

            logger.LogError($"Error on deleting register Error: {ex.Message}");
            return TypedResults.BadRequest();
        }

    }
}
