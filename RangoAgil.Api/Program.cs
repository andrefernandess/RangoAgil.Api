using Microsoft.EntityFrameworkCore;
using RangoAgil.Api.RangoDbContext;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<RangoContext>(
    o => o.UseSqlite(builder.Configuration["ConnectionStrings:RangoAgilSqlite"]));

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
