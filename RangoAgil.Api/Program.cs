using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RangoAgil.Api.DTOs;
using RangoAgil.Api.Entities;
using RangoAgil.Api.RangoDbContext;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<RangoContext>(
    o => o.UseSqlite(builder.Configuration["ConnectionStrings:RangoAgilSqlite"]));


builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

//Route Group
var rangosGroup = app.MapGroup("/rangos");
var rangosWithIdGroup = rangosGroup.MapGroup("/{rangoId:int}");

app.MapGet("/", () => "Hello World!");

rangosWithIdGroup.MapGet("", async (
    RangoContext context, 
    IMapper mapper,
    int rangoId) =>
{
    var result = mapper.Map<RangoDTO>(await context.Rangos.FirstOrDefaultAsync(x => x.Id == rangoId));
    return result;
}).WithName("GetRangos");

app.MapGet("/rangos/{name}", async (
    RangoContext context, 
    IMapper mapper, 
    string name) =>
{
    var result = await context.Rangos.Where(x => x.Name == name).ToListAsync();

    var response = mapper.Map<IEnumerable<RangoDTO>>(result);
    return response;
});

rangosGroup.MapGet("", async Task<Results<NoContent, Ok<List<RangoDTO>>>> 
    (
        RangoContext context,
        IMapper mapper,
        [FromQuery(Name = "name")] string? name
    ) =>
{
    
    var result = mapper.Map<IEnumerable<RangoDTO>>(await context.Rangos
                        .Where(x => name == null || x.Name.ToLower().Contains(name.ToLower())).ToListAsync());

    if (!result.Any() || result == null)
        return TypedResults.NoContent();
    else
        return TypedResults.Ok(result.ToList());
});

rangosGroup.MapGet("/ingredients", async Task<Results<NoContent, Ok<List<IngredientDTO>>>>
    (
        RangoContext context,
        IMapper mapper,
        int rangoId
    ) =>
{
    var result = mapper.Map<IEnumerable<IngredientDTO>>((await context.Rangos
                        .Include(rango => rango.Ingredients)
                        .FirstOrDefaultAsync(rango => rango.Id == rangoId))?.Ingredients).ToList();


    if (!result.Any() || result == null)
        return TypedResults.NoContent();
    else
        return TypedResults.Ok(result);
});

//Uma das formas de fazer, porem mais longa e com dependencias desnecessarias
//app.MapPost("/rangoss", async ( RangoContext context,
//    IMapper mapper,
//    [FromBody] CreateRangoDTO createRangoDTO,
//    LinkGenerator linkGenerator,
//    HttpContext httpContext) =>
//{
//    var entity = mapper.Map<Rango>(createRangoDTO);

//    context.Add(entity);
//    await context.SaveChangesAsync();

//    var result = mapper.Map<RangoDTO>(entity);

//    var linkToReturn = linkGenerator.GetUriByName(
//         httpContext,
//        "GetRango",
//        new { id = result.Id }
//    );

//    return TypedResults.Created(linkToReturn, result);
//});

rangosGroup.MapPost("", async (
    RangoContext context,
    IMapper mapper,
    [FromBody] CreateRangoDTO createRangoDTO
) =>
{
    var entity = mapper.Map<Rango>(createRangoDTO);

    context.Add(entity);
    await context.SaveChangesAsync();

    var result = mapper.Map<RangoDTO>(entity);

    return TypedResults.CreatedAtRoute(result, "GetRangos", new { rangoId = result.Id });
});


rangosWithIdGroup.MapPut("", async Task<Results<NotFound, Ok>> (
    RangoContext context,
    IMapper mapper,
    [FromBody] UpdateRangoDTO updateRangoDTO,
    int rangoId) =>
{
    var entity = await context.Rangos.FirstOrDefaultAsync(x => x.Id == rangoId);

    if (entity == null)
        return TypedResults.NotFound();

    var entityToUpdate = mapper.Map(updateRangoDTO, entity);

    await context.SaveChangesAsync();

    return TypedResults.Ok();
});

rangosWithIdGroup.MapDelete("", async Task<Results<NotFound, NoContent>> (
    RangoContext context, int rangoId) =>
{
    var entity = context.Rangos.FirstOrDefault(x => x.Id == rangoId);

    if(entity == null)
        return TypedResults.NotFound();

    context.Rangos.Remove(entity);
    await context.SaveChangesAsync();

    return TypedResults.NoContent();
});

app.Run();
