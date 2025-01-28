using System;

namespace Neama.Dtos
{
    public class OrderResponseDto
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int SurpriseBagId { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime PickupTime { get; set; }
        public string UserDisplayName { get; set; }
        public string SupplierName { get; set; }
        public string SurpriseBagTitle { get; set; }
    }
}
