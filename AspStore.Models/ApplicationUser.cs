using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace AspStore.Models
{
    public class ApplicationUser: IdentityUser
    {
        [Required]
        [StringLength(maximumLength: 25, MinimumLength = 2)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(maximumLength: 25, MinimumLength = 1)]
        public string LastName { get; set; }

        public IEnumerable<Cart> Carts { get; set; }
    }
}
