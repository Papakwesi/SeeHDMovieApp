using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SeeHdWeb.Models
{
    public class WhileList
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int MoviesId { get; set; }

        [ForeignKey("MoviesId")]
        [ValidateNever]
        public Movies Movies { get; set; }

        [Required]
        public string ApplicationUserId { get; set; }

        [ForeignKey("ApplicationUserId")]
        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }

        public bool IsAdded { get; set; } = true;
    }
}
