namespace Neama.Dtos
{
    public class SupplierResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public string PickupStartTime { get; set; }
        public string PickupEndTime { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string UserId { get; set; }
    }
}
