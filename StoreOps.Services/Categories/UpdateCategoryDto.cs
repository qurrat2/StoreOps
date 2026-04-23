using System.ComponentModel.DataAnnotations;

namespace StoreOps.Services.Categories;

public record UpdateCategoryDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required.")]
    [MaxLength(100, ErrorMessage = "Name must be 100 characters or fewer.")]
    public string Name { get; set; } = string.Empty;

    [MaxLength(500, ErrorMessage = "Description must be 500 characters or fewer.")]
    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;
}
