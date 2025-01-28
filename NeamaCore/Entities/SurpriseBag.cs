using System;

namespace Neama.Core.Entities
{
    public class SurpriseBag
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public DateTime PickupTime { get; set; }
        public int QuantityAvailable { get; set; } // Use for soft delete

        public int SupplierId { get; set; }
        public Supplier Supplier { get; set; }
    }
}
