using System.ComponentModel.DataAnnotations;

namespace MvcWithApi.Models.Party
{
    public class Party
    {
        [Key]
        public int partyId { get; set; }

        [Required]
        [StringLength(15, ErrorMessage = "Code cannot exceed 15 character")]
        public string code { get; set; } = string.Empty;

        [Required]
        [StringLength(100, ErrorMessage = "Name is required")]
        public string name { get; set; } = string.Empty;

        [Required]
        [StringLength(200, ErrorMessage = "Address cannot exceed 200 character")]
        public string address { get; set; } = string.Empty;
    }
}
