using System.ComponentModel.DataAnnotations;

namespace Blog.Shared;

public class CreateCategoryRequest
{
    [Required]
    public string Name { get; set; }
}
