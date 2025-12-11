using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace RangoAgil.Api.Entities;

public class Rango
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public required string Name { get; set; }

    public ICollection<Ingredient> Ingredients { get; set; } = [];

    public Rango()
    {
        
    }

    [SetsRequiredMembers]
    public Rango(int id, string name)
    {
        
    }
}
