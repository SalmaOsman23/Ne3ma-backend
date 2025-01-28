using System.ComponentModel.DataAnnotations;

namespace Neama.Dtos
{
    public class OrderCreateDto
    {
        [Required]
        public int SurpriseBagId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }
    }
}
