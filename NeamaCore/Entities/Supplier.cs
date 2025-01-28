using Neama.Core.Entities.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Neama.Core.Entities
{
    public class Supplier  // restaurants, bakeries, supermarkets
    {

        public int Id { get; set; }
        [StringLength(255)]
        public string Name { get; set; }
        [MaxLength(255)]
        public string Address { get; set; }
        [MaxLength(500)]
        public string Description { get; set; }
        public string UserId { get; set; }
        public bool IsActive { get; set; } = true;
        public string PickupStartTime { get; set; }
        public string PickupEndTime { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public AppUser User { get; set; } // Navigation property
        public ICollection<SurpriseBag> SurpriseBags { get; set; }

    }
}
