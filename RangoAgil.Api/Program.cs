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

app.MapGet("/", () => "Hello World!");

//app.MapGet("/rangos", async (RangoContext context) =>
//{
//    return await context.Rangos.ToListAsync();
//});

app.MapGet("/rango/{id:int}", async (
    RangoContext context, 
    IMapper mapper,
    int id
    ) =>
{
    var result = mapper.Map<RangoDTO>(await context.Rangos.FirstOrDefaultAsync(x => x.Id == id));
    return result;
});

app.MapGet("/rango/{name}", async (
    RangoContext context, 
    IMapper mapper, 
    string name) =>
{
    var result = mapper.Map<IEnumerable<RangoDTO>>(await context.Rangos.FirstOrDefaultAsync(x => x.Name == name));
    return result;
});

app.MapGet("/rangos", async Task<Results<NoContent, Ok<List<RangoDTO>>>> 
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

app.MapGet("/rango/{rangoId:int}/ingredients", async Task<Results<NoContent, Ok<List<IngredientDTO>>>>
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

app.Run();
