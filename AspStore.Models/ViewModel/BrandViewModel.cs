using AspStore.Models.CustomValidators;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace AspStore.Models.ViewModel
{
    /// <summary>
    /// This is a View Model of <see cref="Brand">Brand</see> model.
    /// </summary>
    public class BrandViewModel
    {
        [Required]
        [StringLength(maximumLength: 50, MinimumLength = 2)]
        public string Name { get; set; }

        [Required]
        [SlugValidator]
        [StringLength(maximumLength: 50, MinimumLength = 2)]
        public string Slug { get; set; }

        [Required]
        [DataType(DataType.Upload)]
        public IFormFile Logo { get; set; }

        [Required]
        [StringLength(maximumLength: 25000, MinimumLength = 5)]
        public string Description { get; set; }

        [Url]
        [StringLength(maximumLength: 150, MinimumLength = 5)]
        [DataType(DataType.Url)]
        public string? OffictalWebsite { get; set; }

        [EmailAddress]
        [Required]
        [StringLength(maximumLength: 150, MinimumLength = 5)]
        [DataType(DataType.EmailAddress)]
        public string SupportEmail { get; set; }
    }
}
