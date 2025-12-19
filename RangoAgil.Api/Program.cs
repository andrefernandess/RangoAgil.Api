using Microsoft.EntityFrameworkCore;
using RangoAgil.Api.Extensions;
using RangoAgil.Api.RangoDbContext;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<RangoContext>(
    o => o.UseSqlite(builder.Configuration["ConnectionStrings:RangoAgilSqlite"]));


builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddProblemDetails();

var app = builder.Build();

if(!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler();

    //For custom error handling uncomment below and comment the line above
    //app.UseExceptionHandler(configureExceptionBuilder =>
    //{
    //    configureExceptionBuilder.Run(
    //       async context =>
    //       {
    //           context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
    //           context.Response.ContentType = "text/html";
    //           await context.Response.WriteAsync("An unexpected error happened");
    //       }
    //     );
    //});
}

app.RegisterRangoEndpoints();
app.RegisterIngredientEndpoints();


app.Run();
