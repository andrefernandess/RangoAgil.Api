using RangoAgil.Api.EndPointHandlers;

namespace RangoAgil.Api.Extensions;

public static class EndPointRouteBuilderExtension
{
    public static void RegisterRangoEndpoints(this IEndpointRouteBuilder app)
    {
        var rangosGroup = app.MapGroup("/rangos");
        var rangosWithIdGroup = rangosGroup.MapGroup("/{rangoId:int}");

        rangosWithIdGroup.MapGet("", RangoHandler.GetRangoByIdAsync).WithName("GetRangos");

        rangosGroup.MapGet("/rangos/{name}", RangoHandler.GetRangoByNameAsync);

        rangosGroup.MapGet("", RangoHandler.GetRangosAsync);

        rangosGroup.MapPost("", RangoHandler.CreateRangoAsync);

        rangosWithIdGroup.MapPut("", RangoHandler.UpdateRangoAsync);

        rangosWithIdGroup.MapDelete("", RangoHandler.DeleteRangoAsync);
    }

    public static void RegisterIngredientEndpoints(this IEndpointRouteBuilder app)
    {
        var ingredientsGroup = app.MapGroup("/rangos/{rangoId:int}/ingredients");
        ingredientsGroup.MapGet("", IngredientHandler.GetIngredientsAsync).WithName("GetIngredients");
        //ingredientsGroup.MapGet("/{ingredientId:int}", IngredientHandler.GetIngredientByIdAsync);
        //ingredientsGroup.MapGet("/name/{name}", IngredientHandler.GetIngredientByNameAsync);
        //ingredientsGroup.MapPost("", IngredientHandler.CreateIngredientAsync);
        //ingredientsGroup.MapPut("/{ingredientId:int}", IngredientHandler.UpdateIngredientAsync);
        //ingredientsGroup.MapDelete("/{ingredientId:int}", IngredientHandler.DeleteIngredientAsync);
    }
}
