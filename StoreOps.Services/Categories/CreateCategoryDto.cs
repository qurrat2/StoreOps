using System.ComponentModel.DataAnnotations;

namespace StoreOps.Services.Categories;

public record CreateCategoryDto
{
    [Required(ErrorMessage = "Name is required.")]
    [MaxLength(100, ErrorMessage = "Name must be 100 characters or fewer.")]
    public string Name { get; set; } = string.Empty;

    [MaxLength(500, ErrorMessage = "Description must be 500 characters or fewer.")]
    public string? Description { get; set; }
}
