using System.ComponentModel.DataAnnotations;

namespace ToDoApp.Models
{
    public class ToDo
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please provide a name.")]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(250)]
        public string? Description { get; set; }

        [Required]
        [Display(Name = "Complete On")]
        public DateTime CompleteOn { get; set; }

        public bool HasCompleted { get; set; }
    }
}
