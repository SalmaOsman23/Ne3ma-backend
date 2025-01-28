using System.ComponentModel.DataAnnotations;

namespace Neama.Dtos
{
    public class SupplierCreateDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Address { get; set; }

        public string Description { get; set; }

        [Required]
        public string PickupStartTime { get; set; }

        [Required]
        public string PickupEndTime { get; set; }

        [Required]
        public double Latitude { get; set; }

        [Required]
        public double Longitude { get; set; }
    }
}
