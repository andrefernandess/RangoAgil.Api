using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace RangoAgil.Api.Entities;

public class Ingredient
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public required string Name { get; set; }

    public ICollection<Rango> Rangos { get; set; } = [];

    public Ingredient()
    {
        
    }

    [SetsRequiredMembers]
    public Ingredient(int id, string name)
    {
        
    }
}
