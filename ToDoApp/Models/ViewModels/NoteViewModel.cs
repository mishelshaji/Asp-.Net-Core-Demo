using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToDoApp.Models.ViewModels
{
    public class NoteViewModel
    {
        [Required]
        [StringLength(maximumLength: 70, MinimumLength = 2)]
        public string Title { get; set; }

        [Required]
        [StringLength(15000)]
        public string Body { get; set; }

        [DataType(DataType.DateTime)] // Optional
        public DateTime? ExpiresOn { get; set; }

        [Display(Name = "category")]
        public int CategoryId { get; set; }
        public IEnumerable<Category> Categories { get; set; }
    }
}
