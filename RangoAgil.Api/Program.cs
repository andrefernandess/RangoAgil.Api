using Microsoft.EntityFrameworkCore;
using RangoAgil.Api.EndPointHandlers;
using RangoAgil.Api.Extensions;
using RangoAgil.Api.RangoDbContext;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<RangoContext>(
    o => o.UseSqlite(builder.Configuration["ConnectionStrings:RangoAgilSqlite"]));


builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

app.RegisterRangoEndpoints();
app.RegisterIngredientEndpoints();


app.Run();
