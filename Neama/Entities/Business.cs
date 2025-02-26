using NetTopologySuite.Geometries;

namespace Ne3ma.Entities;

public class Business : AuditableEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public Point Location { get; set; } = new Point(0, 0) { SRID = 4326 };
    public string Phone { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string LogoUrl { get; set; } = string.Empty;
    public bool IsApproved { get; set; }

    public string UserId { get; set; } = string.Empty;
    public ApplicationUser User { get; set; } = null!;

    public ICollection<FoodItem> FoodItems { get; set; } = [];
}