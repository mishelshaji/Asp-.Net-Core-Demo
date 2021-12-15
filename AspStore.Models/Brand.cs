using AspStore.Models.CustomValidators;
using System.ComponentModel.DataAnnotations;

namespace AspStore.Models
{
    [Index(nameof(Name), IsUnique = true)]
    [Index(nameof(Slug), IsUnique = true)]
    [Index(nameof(SupportEmail), IsUnique = true)]
    public class Brand
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [StringLength(maximumLength: 50, MinimumLength = 2)]
        public string Name { get; set; }

        [Required]
        [SlugValidator]
        [StringLength(maximumLength: 50, MinimumLength = 2)]
        public string Slug { get; set; }

        [Required]
        [StringLength(maximumLength: 250, MinimumLength = 5)]
        public string Logo { get; set; }

        [Required]
        [StringLength(maximumLength: 25000, MinimumLength = 5)]
        public string Description { get; set; }

        [Url]
        [StringLength(maximumLength: 150, MinimumLength = 5)]
        public string? OffictalWebsite { get; set; }

        [EmailAddress]
        [Required]
        [StringLength(maximumLength: 150, MinimumLength = 5)]
        public string SupportEmail { get; set; }

        public IEnumerable<Product> Products { get; set; }
    }
}
