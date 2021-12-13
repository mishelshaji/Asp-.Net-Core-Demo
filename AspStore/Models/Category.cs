using AspStore.Models.CustomValidators;
using System.ComponentModel.DataAnnotations;

namespace AspStore.Models;

public class Category
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(maximumLength: 50, MinimumLength = 2)]
    public string Name { get; set; }

    [Required]
    [StringLength(maximumLength: 50, MinimumLength = 2)]
    [SlugValidator]
    public string Slug { get; set; }
}
