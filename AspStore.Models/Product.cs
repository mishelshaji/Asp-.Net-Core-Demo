using AspStore.Models.CustomValidators;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AspStore.Models
{
    [Index(nameof(Name), IsUnique = true)]
    [Index(nameof(Slug), IsUnique = true)]
    public class Product
    {
        [Key]
        public long Id { get; set; }

        [Required]
        [StringLength(maximumLength: 70, MinimumLength = 5)]
        public string Name { get; set; }

        [Required]
        [SlugValidator]
        [StringLength(maximumLength: 70, MinimumLength = 5)]
        public string Slug { get; set; }

        [Required]
        [Range(0, 150000)]
        public int RegularPrice { get; set; }

        [Required]
        [Range(0, 150000)]
        public int SalesPrice { get; set; }

        [Required]
        [StringLength(maximumLength: 150, MinimumLength = 5)]
        public string Image { get; set; }

        [Required]
        [StringLength(maximumLength: 250, MinimumLength = 10)]
        public string MetaDescription { get; set; }

        [Required]
        [StringLength(maximumLength: 4000, MinimumLength = 10)]
        public string Description { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime UpdatedOn { get; set; }

        [Required]
        public int CategoryId { get; set; }
        [ForeignKey(nameof(CategoryId))]
        public Category Category { get; set; }

        public int BrandId { get; set; }
        [ForeignKey(nameof(BrandId))]
        public Brand Brand { get; set; }

        public string ApplicationUserId { get; set; }
        [ForeignKey(nameof(ApplicationUserId))]
        public ApplicationUser ApplicationUser { get; set; }
    }
}
