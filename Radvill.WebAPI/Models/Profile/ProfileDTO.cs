using System.ComponentModel.DataAnnotations;

namespace Radvill.WebAPI.Models.Profile
{
    public class ProfileDTO
    {
        [Required]
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public int Points { get; set; }
    }
}