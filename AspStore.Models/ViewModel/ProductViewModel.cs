using AspStore.Models.CustomValidators;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspStore.Models.ViewModel;

public class ProductViewModel
{
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
    public IFormFile Image { get; set; }

    [Required]
    [StringLength(maximumLength: 250, MinimumLength = 10)]
    public string MetaDescription { get; set; }

    [Required]
    [StringLength(maximumLength: 4000, MinimumLength = 10)]
    public string Description { get; set; }

    [Required]
    public int CategoryId { get; set; }
    public IEnumerable<Category>? Categories { get; set; }

    public int BrandId { get; set; }
    public IEnumerable<Brand>? Brands { get; set; }
}
