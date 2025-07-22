using System.ComponentModel.DataAnnotations;

namespace SeeHdWeb.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Description is required.")]
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string Description { get; set; }
        public DateTime CreatedDateTime { get; set; } = DateTime.Now;
    }
}
