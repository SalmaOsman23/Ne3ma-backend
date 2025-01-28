using System;

namespace Neama.Dtos
{
    public class SurpriseBagResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public DateTime PickupTime { get; set; }
        public int QuantityAvailable { get; set; }
        public int SupplierId { get; set; }
    }
}
