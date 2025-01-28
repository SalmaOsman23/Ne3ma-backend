using Neama.Core.Entities.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace Neama.Core.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int SurpriseBagId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }

        public decimal TotalPrice => Quantity * SurpriseBag.Price;
        public DateTime PickupTime { get; set; }

        public AppUser User { get; set; }
        public SurpriseBag SurpriseBag { get; set; }
    }
}
