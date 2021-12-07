using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToDoApp.Models;

[Index(nameof(Title), IsUnique = true)]
public class Note
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(maximumLength: 70, MinimumLength = 2)]
    public string Title { get; set; }

    [Required]
    [StringLength(15000)]
    public string Body { get; set; }

    public DateTime? ExpiresOn { get; set; }

    public int CategoryId { get; set; }
    [ForeignKey(nameof(CategoryId))]
    public Category Category { get; set; }
}
