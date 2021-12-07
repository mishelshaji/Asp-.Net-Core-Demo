using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ToDoApp.Models;

[Index(nameof(Name), IsUnique = true)]
public class Category
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(maximumLength: 25, MinimumLength = 2)]
    public string Name { get; set; }

    [StringLength(maximumLength: 250, MinimumLength = 0)]
    public string? Description { get; set; }
    
    public IEnumerable<Note> Notes { get; set; }
}
