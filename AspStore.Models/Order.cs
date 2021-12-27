using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspStore.Models
{
    public class Order
    {
        public long Id { get; set; }

        [Required]
        [StringLength(maximumLength: 50)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(maximumLength: 150)]
        public string Email { get; set; }

        [Required]
        [StringLength(maximumLength: 500)]
        public string Address { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public double TotalAmount { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        [Required]
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public IEnumerable<OrderItem> OrderItems { get; set; }
    }
}
