using System;
using System.ComponentModel.DataAnnotations;

namespace Neama.Dtos
{
    public class SurpriseBagCreateDto
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public DateTime PickupTime { get; set; }
        [Required]
        public int QuantityAvailable { get; set; }
    }
}
