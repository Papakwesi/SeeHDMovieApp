using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace SeeHdWeb.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string Name { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-mm-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        public string? Gender { get; set; }
        public string? Country { get; set; }
        public string? FavoriteGenres { get; set; }
    }
}
