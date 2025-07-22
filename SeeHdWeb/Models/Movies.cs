using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SeeHdWeb.Models
{
    public class Movies
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(25000)]
        [Required]

        public string Title { get; set; }
        [Column(TypeName = "nvarchar(MAX)")]
        [Required]
        public string Description { get; set; }
        [Required]
        public string Genre { get; set; }
        [Range(1,5, ErrorMessage = "Rating must be between 1 and 5")]
        public int Rating { get; set; }
        [Display(Name = "Release Date")]
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }
        [ValidateNever]
        public string ImageUrl { get; set; }
        [ValidateNever]
        [ForeignKey("CategoryId")]
        public int CategoryId { get; set; }
        [ValidateNever]
        public Category Category { get; set; }

        public int CoverTypeId { get; set; }
        [ValidateNever]
        public CoverType CoverType { get; set; }

    }
}
